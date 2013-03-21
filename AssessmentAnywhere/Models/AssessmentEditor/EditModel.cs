namespace AssessmentAnywhere.Models.AssessmentEditor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AssessmentAnywhere.Services;
    using AssessmentAnywhere.Services.Assessments;
    using AssessmentAnywhere.Services.GradeBoundaries;

    public class EditModel
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public IList<ResultRow> Results { get; private set; }

        public decimal? TotalMarks { get; private set; }

        public ResultRow NewRow { get; private set; }

        public int? SelectedResultIndex { get; private set; }

        public EditModel(Guid id, string name, IList<ResultRow> results, decimal? totalMarks, ResultRow newRow, int? selectedResultIndex)
        {
            this.Id = id;
            this.Name = name;
            this.Results = results;
            this.TotalMarks = totalMarks;
            this.NewRow = newRow;
            this.SelectedResultIndex = selectedResultIndex;
        }

        public EditModel(Guid id, string name, IList<ResultRow> results, decimal? totalMarks, int? selectedResultIndex)
            : this(id, name, results, totalMarks, ResultRow.NewRow, selectedResultIndex)
        {
        }

        public EditModel(IAssessment assessment, int? lastSelectedResult)
            : this(
                assessment.Id,
                assessment.Name,
                GenerateResultsFromRepoModels(assessment),
                assessment.TotalMarks,
                GetSelectedResultIndex(lastSelectedResult, assessment.Results))
        {
        }

        public EditModel(
            IAssessment assessment,
            IGradeBoundaries boundaries,
            int? lastSelectedResult)
            : this(
                assessment.Id,
                assessment.Name,
                GenerateResultsFromRepoModels(assessment, boundaries),
                assessment.TotalMarks,
                GetSelectedResultIndex(lastSelectedResult, assessment.Results))
        {
        }

        public EditModel(Guid id, UpdateModel model, IGradeBoundaries boundaries)
            : this(
                id,
                model.Name,
                GenerateResultsFromUpdateModel(model, boundaries),
                model.TotalMarks,
                new ResultRow(Guid.Empty, model.NewRow.Surname, model.NewRow.Forenames, null, null),
                null)
        {
        }

        public EditModel(Guid id, UpdateModel model)
            : this(
                id,
                model.Name,
                GenerateResultsFromUpdateModel(model),
                model.TotalMarks,
                new ResultRow(Guid.Empty, model.NewRow.Surname, model.NewRow.Forenames, null, null),
                null)
        {
        }

        private static List<ResultRow> GenerateResultsFromUpdateModel(UpdateModel model, IGradeBoundaries boundaries)
        {
            if (model.Results == null)
            {
                return new List<ResultRow>();
            }

            return model.Results.Select(
                r =>
                new ResultRow(
                    r.RowId,
                    r.Surname,
                    r.Forenames,
                    r.Result,
                    r.Result / model.TotalMarks * 100,
                    boundaries.Boundaries.ForResult(r.Result))).ToList();
        }

        private static List<ResultRow> GenerateResultsFromUpdateModel(UpdateModel model)
        {
            if (model.Results == null)
            {
                return new List<ResultRow>();
            }

            return model.Results.Select(
                r =>
                new ResultRow(
                    r.RowId,
                    r.Surname,
                    r.Forenames,
                    r.Result,
                    r.Result / model.TotalMarks * 100,
                    string.Empty)).ToList();
        }

        private static IList<ResultRow> GenerateResultsFromRepoModels(
            IAssessment assessment, IGradeBoundaries boundaries)
        {
            var results = from result in assessment.Results
                          let grade = boundaries.Boundaries.ForResult(result.Result)
                          let percentage = result.Result / assessment.TotalMarks * 100
                          select new ResultRow(result.Id, result.Surname, result.Forenames, result.Result, percentage, grade);

            return results.ToList();
        }

        private static IList<ResultRow> GenerateResultsFromRepoModels(IAssessment assessment)
        {
            var results = from result in assessment.Results
                          let percentage = result.Result / assessment.TotalMarks * 100
                          select new ResultRow(result.Id, result.Surname, result.Forenames, result.Result, percentage);
            return results.ToList();
        }

        private static int? GetSelectedResultIndex(int? lastSelectedResult, IList<IAssessmentResult> results)
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
