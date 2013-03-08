namespace AssessmentAnywhere.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using AssessmentAnywhere.Services.Models;
    using AssessmentAnywhere.Services.Repos;
    using AssessmentAnywhere.Services.Repos.Models;

    public class AssessmentService
    {
        public AssessmentGrades GetAssessmentGrades(Guid assessmentId)
        {
            var assessmentRepo = new AssessmentsRepo();
            var gradeBoundariesRepo = new GradeBoundariesRepo();

            var assessment = assessmentRepo.Open(assessmentId);

            bool hasBoundaries;
            var gradeBoundaries = gradeBoundariesRepo.TryOpen(assessmentId, out hasBoundaries);

            if (hasBoundaries)
                return ConstructAssessmentGrades(assessment, gradeBoundaries.Boundaries);

            return ConstructAssessmentGrades(assessment, Enumerable.Empty<Boundary>());
        }

        public IEnumerable<AssessmentStatistic> GetStatsForAssessmentGroup(Guid assessmentGroupId, bool includeGradeCounts)
        {
            var assessmentGroupRepo = new AssessmentGroupRepo();
            var assessmentGroup = assessmentGroupRepo.Open(assessmentGroupId);

            if (assessmentGroup == null || assessmentGroup.AssessmentIds == null)
            {
                return new List<AssessmentStatistic>();
            }

            var candidateGrades = new List<CandidateGrade>();

            foreach (var id in assessmentGroup.AssessmentIds)
            {
                var assessment = this.GetAssessmentGrades(id);
                if (assessment == null)
                {
                    continue;
                }

                if (assessment.Candidates != null)
                {
                    candidateGrades.AddRange(assessment.Candidates);
                }
            }

            return PopulateStatsList(candidateGrades, assessmentGroup.Boundaries.Boundaries, includeGradeCounts);
        }

        public IEnumerable<AssessmentStatistic> GetStatsForAssessment(Guid assessmentId, bool includeGradeCounts)
        {
            var assessment = this.GetAssessmentGrades(assessmentId);
            if (assessment == null)
            {
                return new List<AssessmentStatistic>();
            }

            return PopulateStatsList(assessment.Candidates, assessment.Boundaries, includeGradeCounts);
        }

        public Dictionary<string, int> GetGradeCounts(Guid assessmentId)
        {
            var results = this.GetAssessmentGrades(assessmentId);
            if (results == null)
            {
                return new Dictionary<string, int>();
            }

            return GetGradeCounts(results.Candidates, results.Boundaries);
        }

        public Dictionary<string, int> GetGradeCountsForGroup(Guid groupAssessmentId)
        {
            var assessmentGroupRepo = new AssessmentGroupRepo();
            var assessmentGroup = assessmentGroupRepo.Open(groupAssessmentId);
            if (assessmentGroup == null || assessmentGroup.AssessmentIds == null)
            {
                return new Dictionary<string, int>();
            }

            var candidateGrades = new List<CandidateGrade>();
            foreach (var id in assessmentGroup.AssessmentIds)
            {
                var assessmentGrades = this.GetAssessmentGrades(id);
                if (assessmentGrades == null || assessmentGrades.Candidates == null)
                {
                    continue;
                }

                candidateGrades.AddRange(assessmentGrades.Candidates);
            }

            return GetGradeCounts(candidateGrades, assessmentGroup.Boundaries.Boundaries);
        }

        private static IEnumerable<AssessmentStatistic> PopulateStatsList(
            IEnumerable<CandidateGrade> candidateGrades, IEnumerable<Boundary> boundaries, bool includeGradeCounts)
        {
            var stats = new List<AssessmentStatistic>();

            if (candidateGrades == null || boundaries == null)
            {
                return stats;
            }

            stats.Add(AverageScore(candidateGrades));
            stats.Add(HighestScore(candidateGrades));
            stats.Add(LowestScore(candidateGrades));
            if (includeGradeCounts)
            {
                stats.AddRange(GradeCounts(candidateGrades, boundaries));
            }

            return stats;
        }

        private static AssessmentStatistic AverageScore(IEnumerable<CandidateGrade> candidateGrades)
        {
            var stat = new AssessmentStatistic { StatisticName = "Average", StatisticValue = 0.ToString(CultureInfo.InvariantCulture) };

            if (candidateGrades == null)
            {
                return stat;
            }

            decimal total = 0;
            int count = 0;
            foreach (var candidate in candidateGrades)
            {
                if (candidate != null && candidate.Result.HasValue)
                {
                    total += candidate.Result.Value;
                    count++;
                }
            }

            if (count > 0) stat.StatisticValue = (total / count).ToString(CultureInfo.InvariantCulture);
            else stat.StatisticValue = "";
            return stat;
        }

        private static AssessmentStatistic HighestScore(IEnumerable<CandidateGrade> candidateGrades)
        {
            var stat = new AssessmentStatistic { StatisticName = "Highest Score", StatisticValue = 0.ToString(CultureInfo.InvariantCulture) };

            if (candidateGrades == null)
            {
                return stat;
            }

            var max = (from grade in candidateGrades
                       let result = grade.Result
                       where result != null
                       where grade != null && result.HasValue
                       select result.Value).Concat(new decimal[] { 0 }).Max();
            stat.StatisticValue = max.ToString(CultureInfo.InvariantCulture);

            return stat;
        }

        private static AssessmentStatistic LowestScore(IEnumerable<CandidateGrade> candidateGrades)
        {
            var stat = new AssessmentStatistic { StatisticName = "Lowest Score", StatisticValue = 0.ToString(CultureInfo.InvariantCulture) };

            if (candidateGrades == null)
            {
                return stat;
            }

            decimal min = 100;
            
            foreach (var candidate in candidateGrades)
            {
                if (candidate != null && candidate.Result.HasValue && candidate.Result.Value < min)
                {
                    min = candidate.Result.Value;
                }
            }

            stat.StatisticValue = min.ToString(CultureInfo.InvariantCulture);

            return stat;
        }

        private static IEnumerable<AssessmentStatistic> GradeCounts(IEnumerable<CandidateGrade> candidateGrades, IEnumerable<Boundary> boundaries)
        {
            var stats = new List<AssessmentStatistic>();

            var gradeCounts = GetGradeCounts(candidateGrades, boundaries);
            stats.AddRange(gradeCounts.Select(gradeCount => new AssessmentStatistic
                {
                    StatisticName = gradeCount.Key,
                    StatisticValue = gradeCount.Value.ToString(CultureInfo.InvariantCulture)
                }));

            return stats;
        }

        private static Dictionary<string, int> GetGradeCounts(IEnumerable<CandidateGrade> candidateGrades, IEnumerable<Boundary> boundaries)
        {
            var results = new Dictionary<string, int>();

            if (candidateGrades == null || boundaries == null)
            {
                return results;
            }

            foreach (var boundary in boundaries)
            {
                if (boundary != null)
                {
                    results.Add(boundary.Grade, 0);
                }
            }

            foreach (var grade in candidateGrades)
            {
                if (grade != null)
                {
                    if (!results.ContainsKey(grade.Grade))
                    {
                        results.Add(grade.Grade, 1);
                    }
                    else
                    {
                        results[grade.Grade] = results[grade.Grade] + 1;
                    }
                }
            }

            return results;
        }

        private static AssessmentGrades ConstructAssessmentGrades(
            Assessment assessment, IEnumerable<Boundary> boundaries)
        {
            return new AssessmentGrades(
                assessment.Id,
                assessment.Name,
                null,
                assessment.Results.Select(result => ConstructCandidateGrade(boundaries, result)).ToList(),
                boundaries);
        }

        private static CandidateGrade ConstructCandidateGrade(IEnumerable<Boundary> boundaries, AssessmentResult result)
        {
            return new CandidateGrade
                       {
                           Result = result.Result,
                           Name = result.CandidateName,
                           Grade = BoundaryForResult(boundaries, result.Result),
                       };
        }

        private static string BoundaryForResult(IEnumerable<Boundary> boundaries, decimal? result)
        {
            if (result.HasValue)
            {
                var match =
                    boundaries.Where(boundary => result.Value >= boundary.MinResult)
                              .OrderByDescending(boundary => boundary.MinResult)
                              .FirstOrDefault();
                if (match != null)
                {
                    return match.Grade;
                }
            }

            return string.Empty;
        }
    }
}
