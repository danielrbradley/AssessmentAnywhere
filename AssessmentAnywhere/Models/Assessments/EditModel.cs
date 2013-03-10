namespace AssessmentAnywhere.Models.Assessments
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AssessmentAnywhere.Services.Repos.Models;

    public class EditModel
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public IList<ResultRow> Results { get; private set; }

        public decimal? TotalMarks { get; private set; }

        public int? SelectedResultIndex { get; private set; }

        public EditModel(Guid id, string name, IList<ResultRow> results, decimal? totalMarks, int? selectedResultIndex)
        {
            Id = id;
            Name = name;
            Results = results;
            TotalMarks = totalMarks;
            SelectedResultIndex = selectedResultIndex;
        }

        public EditModel(Services.Repos.Models.Assessment assessment, int? lastSelectedResult)
            : this(
                assessment.Id,
                assessment.Name,
                GenerateResultsFromRepoModels(assessment),
                assessment.TotalMarks,
                GetSelectedResultIndex(lastSelectedResult, assessment.Results))
        {
        }

        public EditModel(
            Services.Repos.Models.Assessment assessment,
            Services.Repos.Models.GradeBoundaries boundaries,
            int? lastSelectedResult)
            : this(
                assessment.Id,
                assessment.Name,
                GenerateResultsFromRepoModels(assessment, boundaries),
                assessment.TotalMarks,
                GetSelectedResultIndex(lastSelectedResult, assessment.Results))
        {
        }

        private static IList<ResultRow> GenerateResultsFromRepoModels(
            Services.Repos.Models.Assessment assessment, Services.Repos.Models.GradeBoundaries boundaries)
        {
            var results = from result in assessment.Results
                          let grade = boundaries.Boundaries.ForResult(result.Result)
                          let percentage = result.Result / assessment.TotalMarks * 100
                          select new ResultRow(result.Id, result.Surname, result.Forenames, result.Result, percentage, grade);

            return results.ToList();
        }

        private static IList<ResultRow> GenerateResultsFromRepoModels(Services.Repos.Models.Assessment assessment)
        {
            var results = from result in assessment.Results
                          let percentage = result.Result / assessment.TotalMarks * 100
                          select new ResultRow(result.Id, result.Surname, result.Forenames, result.Result, percentage);
            return results.ToList();
        }

        private static int? GetSelectedResultIndex(int? lastSelectedResult, IList<AssessmentResult> results)
        {
            if (lastSelectedResult.HasValue && lastSelectedResult < results.Count - 1)
            {
                return lastSelectedResult + 1;
            }

            // Select new box
            return null;
        }
    }
}
