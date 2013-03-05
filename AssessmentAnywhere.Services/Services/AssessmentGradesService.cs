using System;
using System.Collections.Generic;

namespace AssessmentAnywhere.Services.Services
{
    using System.Linq;

    using AssessmentAnywhere.Services.Model;
    using AssessmentAnywhere.Services.Repos;

    public class AssessmentGradesService
    {

        public AssessmentGrades GetAssessmentGrades(Guid assessmentId)
        {
            var assessmentRepo = new AssessmentsRepo();
            var registersRepo = new RegistersRepo();
            var gradeBoundariesRepo = new GradeBoundariesRepo();

            var assessment = assessmentRepo.Open(assessmentId);
            var register = registersRepo.Open(assessment.RegisterId);
            var gradeBoundaries = gradeBoundariesRepo.Open(assessmentId);

            return ConstructAssessmentGrades(assessment, register.Candidates, gradeBoundaries.Boundaries);
        }

        public static AssessmentGrades ConstructAssessmentGrades(
            Assessment assessment, List<Candidate> candidates, List<Boundary> boundaries)
        {
            return new AssessmentGrades
                       {
                           AsssessmentId = assessment.Id,
                           AssessmentName = assessment.Name,
                           Candidates =
                               assessment.Results.Select(
                                   result => ConstructCandidateGrade(candidates, boundaries, result))
                                         .ToList(),
                           Boundaries = boundaries,
                       };
        }

        public List<AssessmentStatistic> GetStatsForAssessmentGroup(Guid assessmentGroupId, bool includeGradeCounts)
        {
            
            var assessmentGroupRepo = new AssessmentGroupRepo();
            var assessmentGroup = assessmentGroupRepo.Open(assessmentGroupId);

            if (assessmentGroup == null || assessmentGroup.AssessmentIds == null)
                return new List<AssessmentStatistic>();

            var candidateGrades = new List<CandidateGrade>();

            foreach (var id in assessmentGroup.AssessmentIds)
            {
                var assessment = GetAssessmentGrades(id);
                if (assessment == null) continue;

                if (assessment.Candidates != null)
                    candidateGrades.AddRange(assessment.Candidates);
                    
            }

            return PopulateStatsList(candidateGrades, assessmentGroup.Boundaries.Boundaries, includeGradeCounts);
        }

        public List<AssessmentStatistic> GetStatsForAssessment(Guid assessmentId, bool includeGradeCounts)
        {
            var assessment = GetAssessmentGrades(assessmentId);
            if (assessment == null)
                return new List<AssessmentStatistic>();

            return PopulateStatsList(assessment.Candidates, assessment.Boundaries, includeGradeCounts);
        }

        public Dictionary<string, int> GetGradeCounts(Guid assessmentId)
        {
            var results = GetAssessmentGrades(assessmentId);
            if (results == null)
                return new Dictionary<string, int>();

            return GetGradeCounts(results.Candidates, results.Boundaries);

        }

        public Dictionary<string, int> GetGradeCountsForGroup(Guid groupAssessmentId)
        {
            var assessmentGroupRepo = new AssessmentGroupRepo();
            var assessmentRepo = new AssessmentsRepo();
            var registerRepo = new RegistersRepo();
            var assessmentGroup = assessmentGroupRepo.Open(groupAssessmentId);
            if (assessmentGroup == null || assessmentGroup.AssessmentIds == null)
                return new Dictionary<string, int>();

            var candidateGrades = new List<CandidateGrade>();
            foreach (var id in assessmentGroup.AssessmentIds)
            {
                var assessmentGrades = GetAssessmentGrades(id);
                if (assessmentGrades == null || assessmentGrades.Candidates == null) continue;

                candidateGrades.AddRange(assessmentGrades.Candidates);
            }

            return GetGradeCounts(candidateGrades, assessmentGroup.Boundaries.Boundaries);
        }



        private List<AssessmentStatistic> PopulateStatsList(List<CandidateGrade> candidateGrades,
                                                            List<Boundary> boundaries, bool includeGradeCounts)
        {
            var stats = new List<AssessmentStatistic>();

            if (candidateGrades == null || boundaries == null)
                return stats;

            stats.Add(AverageScore(candidateGrades));
            stats.Add(HighestScore(candidateGrades));
            stats.Add(LowestScore(candidateGrades));
            if (includeGradeCounts)
                stats.AddRange(GradeCounts(candidateGrades, boundaries));

            return stats;
        }

        private static AssessmentStatistic AverageScore(List<CandidateGrade> candidateGrades)
        {
            decimal average;
            var stat = new AssessmentStatistic {StatisticName = "Average", StatisticValue = 0.ToString()};

            if (candidateGrades == null || candidateGrades.Count == 0)
                return stat;

            var total = candidateGrades.Where(candidate => candidate != null && candidate.Result.HasValue)
                                                                    .Aggregate<CandidateGrade, decimal>(0, (current, candidate) => current + current);


            stat.StatisticValue = (total/candidateGrades.Count).ToString();
            return stat;
        }

        private static AssessmentStatistic HighestScore(List<CandidateGrade> candidateGrades)
        {
            var stat = new AssessmentStatistic { StatisticName = "Highest Score", StatisticValue = " - " + 0};

            if (candidateGrades == null || candidateGrades.Count == 0)
                return stat;

            var max = (from grade in candidateGrades 
                           where grade != null && grade.Result.HasValue 
                           select grade.Result.Value).Concat(new decimal[] {0}).Max();
            stat.StatisticValue = max.ToString();

            return stat;
        }

        private static AssessmentStatistic LowestScore(List<CandidateGrade> candidateGrades)
        {
            var stat = new AssessmentStatistic { StatisticName = "Lowest Score", StatisticValue = " - " + 0};

            if (candidateGrades == null || candidateGrades.Count == 0)
                return stat;

            decimal min = (from grade in candidateGrades 
                           where grade != null && grade.Result.HasValue 
                           select grade.Result.Value).Concat(new decimal[] {0}).Min();
            stat.StatisticValue = min.ToString();

            return stat;
        }

        private IEnumerable<AssessmentStatistic> GradeCounts(List<CandidateGrade> candidateGrades, List<Boundary> boundaries)
        {
            var stats = new List<AssessmentStatistic>();

            var gradeCounts = GetGradeCounts(candidateGrades, boundaries);
            stats.AddRange(gradeCounts.Select(gradeCount => new AssessmentStatistic
                {
                    StatisticName = gradeCount.Key, StatisticValue = gradeCount.Value.ToString()
                }));

            return stats;
        }

        private Dictionary<string, int> GetGradeCounts(List<CandidateGrade> candidateGrades, List<Boundary> boundaries)
        {
            var results = new Dictionary<string, int>();

            if (candidateGrades == null || candidateGrades.Count == 0 || boundaries == null || boundaries.Count == 0)
                return results;

            var gradeCounts = boundaries.Where(boundary => boundary != null && !string.IsNullOrEmpty(boundary.Grade))
                                .ToDictionary(boundary => boundary.Grade, boundary => 0);


            foreach (var grade in candidateGrades)
            {
                if (grade == null) continue;

                if (!gradeCounts.ContainsKey(grade.Grade))
                    gradeCounts.Add(grade.Grade, 1);
                else
                    gradeCounts[grade.Grade]++;
            }


            return results;

        }

        private static CandidateGrade ConstructCandidateGrade(IEnumerable<Candidate> candidates, IEnumerable<Boundary> boundaries, AssessmentResult result)
        {
            return new CandidateGrade
                       {
                           Id = result.CandidateId,
                           Result = result.Result,
                           Name = candidates.First(candidate => candidate.Id == result.CandidateId).Name,
                           Grade = BoundaryForResult(boundaries, result.Result),
                       };
        }

        private static string BoundaryForResult(IEnumerable<Boundary> boundaries, decimal? result)
        {
            if (result.HasValue)
            {
                return
                    boundaries.Where(boundary => result.Value >= boundary.MinResult)
                              .OrderByDescending(boundary => boundary.MinResult)
                              .First()
                              .Grade;
            }

            return string.Empty;
        }
    }
}
