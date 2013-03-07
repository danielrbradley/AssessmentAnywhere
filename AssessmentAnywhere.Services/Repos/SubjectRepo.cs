namespace AssessmentAnywhere.Services.Repos
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AssessmentAnywhere.Services.Repos.Models;

    public class SubjectRepo
    {
        public static readonly List<CandidateTarget> CandidateTargets = new List<CandidateTarget>();

        public static readonly Dictionary<string, SubjectAssessment> SubjectAssessments = new Dictionary<string, SubjectAssessment>();

        private static readonly List<string> Subjects = new List<string>
            {
               "English",
                "Mathematics",
                "French",
                "Science",
                "Geography",
                "History"
            };

        public List<string> GetSubjects()
        {
            return Subjects;
        }

        public void AddTarget(string candidateName, string subject, string targetGrade)
        {
            foreach (var target in CandidateTargets)
            {
                if (target != null && string.Equals(candidateName, target.CandidateName))
                {
                    if (string.Equals(subject, target.Subject))
                    {
                        target.TargetGrade = targetGrade;
                        break;
                    }
                }

                CandidateTargets.Add(new CandidateTarget
                                         {
                                             CandidateName = candidateName,
                                             Subject = subject,
                                             TargetGrade = targetGrade
                                         });
            }
        }

        public string GetTargetGrade(string candidateName, string subject)
        {
            if (string.IsNullOrEmpty(candidateName) || string.IsNullOrEmpty(subject))
            {
                return string.Empty;
            }

            foreach (
                var target in
                    CandidateTargets.Where(
                        target => target != null && string.Equals(candidateName, target.CandidateName))
                                                    .Where(target => string.Equals(subject, target.Subject)))
            {
                return target.TargetGrade;
            }

            return string.Empty;
        }

        public void AddSubjectAssessment(Guid assessmentId, string subject)
        {
            if (SubjectAssessments.ContainsKey(subject))
            {
                var subjectAssessment = SubjectAssessments[subject];
                if (subjectAssessment.AssessmentIds == null)
                {
                    subjectAssessment.AssessmentIds = new List<Guid>();
                }

                if (!subjectAssessment.AssessmentIds.Contains(assessmentId))
                {
                    subjectAssessment.AssessmentIds.Add(assessmentId);
                }
            }
            else
            {
                SubjectAssessments.Add(
                    subject,
                    new SubjectAssessment { Subject = subject, AssessmentIds = new List<Guid> { assessmentId } });
            }
        }

        public List<Guid> GetAssessmentIdsForSubject(string subject)
        {
            var subjectAssessment = SubjectAssessments[subject];
            if (subjectAssessment == null || subjectAssessment.AssessmentIds == null)
            {
                return new List<Guid>();
            }

            return subjectAssessment.AssessmentIds;
        }

        public string GetSubjectForAssessment(Guid assessmentId)
        {
            foreach (var subjectAssessment in SubjectAssessments.Values
                                .Where(subjectAssessment => subjectAssessment != null && subjectAssessment.AssessmentIds != null)
                                .Where(subjectAssessment => subjectAssessment.AssessmentIds.Contains(assessmentId)))
            {
                return subjectAssessment.Subject;
            }

            return string.Empty;
        }
    }
}
