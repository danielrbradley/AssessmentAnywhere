using System;
using System.Collections.Generic;

namespace AssessmentAnywhere.Services.Services
{
    using System.Linq;

    using AssessmentAnywhere.Services.Model;
    using AssessmentAnywhere.Services.Repos;

    public class AssessmentGradesService
    {
        public AssessmentGrades GetAssessmentGrades(
            Guid assessmentId)
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
