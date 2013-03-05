using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssessmentAnywhere.Services.Model;

namespace AssessmentAnywhere.Services.Repos
{
    public class SubjectRepo
    {
        public static readonly List<CandidateTarget> CandidateTargets = new List<CandidateTarget>();

        public static readonly List<SubjectAssessment> SubjectAssessments = new List<SubjectAssessment>();

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
                var exists = false;
                if (target != null && string.Equals(candidateName, target.CandidateName))
                {
                    if (string.Equals(subject, target.Subject))
                    {
                        target.TargetGrade = targetGrade;
                        exists = true;
                        break;
                    }
                }
                if (!exists)
                {
                    CandidateTargets.Add(new CandidateTarget
                        {
                            CandidateName = candidateName,
                            Subject = subject,
                            TargetGrade = targetGrade
                        });
                }
            }

        }

        public string GetTargetGrade(string candidateName, string subject)
        {            
            if (string.IsNullOrEmpty(candidateName) || string.IsNullOrEmpty(subject))
                return string.Empty;
            foreach (var target in CandidateTargets.Where(target => target != null && string
                                                    .Equals(candidateName, target.CandidateName))
                                                    .Where(target => string.Equals(subject, target.Subject)))
            {
                return target.TargetGrade;
            }
            return string.Empty;
        }

        public void AddSubjectAssessment(Guid assessmentId, string subject)
        {
            foreach (var subjectAssessment in SubjectAssessments)
            {
                if (subjectAssessment != null && string.Equals(subject, subjectAssessment.Subject))
                {
                    if (subjectAssessment.AssessmentIds == null)
                        subjectAssessment.AssessmentIds = new List<Guid>();

                    if (!subjectAssessment.AssessmentIds.Contains(assessmentId))
                        subjectAssessment.AssessmentIds.Add(assessmentId);

                    break;
                }
            }
        }


    }
}
