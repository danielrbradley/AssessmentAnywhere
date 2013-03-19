namespace AssessmentAnywhere.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using AssessmentAnywhere.Models.Assessments;
    using AssessmentAnywhere.Services.Repos;

    [Authorize]
    public class AssessmentEditorController : Controller
    {
        private readonly AssessmentsRepo assessmentsRepo = new AssessmentsRepo();

        private readonly GradeBoundariesRepo gradeBoundariesRepo = new GradeBoundariesRepo();

        // GET: /Assessment/Edit/{id}
        [HttpGet]
        public ActionResult Edit(Guid id, int? lastSelectedResult)
        {
            var assessment = this.assessmentsRepo.Open(id);
            bool hasBoundaries;
            var boundaries = this.gradeBoundariesRepo.TryOpen(id, out hasBoundaries);

            var model = hasBoundaries
                ? new EditModel(assessment, boundaries, lastSelectedResult)
                : new EditModel(assessment, lastSelectedResult);

            return this.View(model);
        }

        // POST: /Assessment/Update/{id}
        [HttpPost]
        public ActionResult Update(Guid id, UpdateModel model)
        {
            if (model.NewRow == null) model.NewRow = new UpdateResultRow();
            if (model.Results == null) model.Results = new List<UpdateResultRow>();

            var assessment = this.assessmentsRepo.Open(id);

            // Validate
            if (!string.IsNullOrWhiteSpace(model.NewRow.Surname)
                || !string.IsNullOrWhiteSpace(model.NewRow.Forenames)
                || model.NewRow.Result.HasValue)
            {
                if (string.IsNullOrWhiteSpace(model.NewRow.Surname))
                {
                    ModelState.AddModelError("NewRow.Surname", "Surname cannot be blank.");
                }
                else if (string.IsNullOrWhiteSpace(model.NewRow.Forenames))
                {
                    ModelState.AddModelError("NewRow.Forenames", "Forenames cannot be blank.");
                }

                if (model.NewRow.Result.HasValue)
                {
                    if (model.NewRow.Result < 0)
                    {
                        ModelState.AddModelError("NewRow.Result", "Result must be a positive number.");
                    }

                    if (model.TotalMarks.HasValue && model.NewRow.Result > model.TotalMarks)
                    {
                        ModelState.AddModelError("NewRow.Result", "Result cannot be greater than the total marks.");
                    }
                }
            }

            foreach (var result in model.Results)
            {
                var keyPrefix = string.Format("Results[{0}].", model.Results.IndexOf(result));
                if (string.IsNullOrWhiteSpace(result.Surname))
                {
                    ModelState.AddModelError(keyPrefix + "Surname", "Surname cannot be blank.");
                }

                if (string.IsNullOrWhiteSpace(result.Forenames))
                {
                    ModelState.AddModelError(keyPrefix + "Forenames", "Forenames cannot be blank.");
                }

                if (result.Result.HasValue)
                {
                    if (result.Result < 0)
                    {
                        ModelState.AddModelError(keyPrefix + "Result", "Result must be a positive number.");
                    }

                    if (model.TotalMarks.HasValue && result.Result > model.TotalMarks)
                    {
                        ModelState.AddModelError(keyPrefix + "Result", "Result cannot be greater than the total marks.");
                    }
                }
            }

            if (!ModelState.IsValid)
            {
                bool hasBoundaries;
                var boundaries = this.gradeBoundariesRepo.TryOpen(id, out hasBoundaries);
                var viewModel = hasBoundaries ? new EditModel(id, model, boundaries) : new EditModel(id, model);

                return this.View("Edit", viewModel);
            }

            if (model.Name != assessment.Name)
            {
                assessment.SetName(model.Name);
            }

            // Check for total marks update.
            if (model.TotalMarks != assessment.TotalMarks)
            {
                assessment.SetTotalMarks(model.TotalMarks);
            }

            // Check for updates
            int? lastSelectedResult = null;
            if (model.Results != null)
            {
                foreach (var modelResult in model.Results)
                {
                    var assessmentResult = assessment.Results.Single(r => r.Id == modelResult.RowId);

                    if (modelResult.Forenames != assessmentResult.Forenames
                        || modelResult.Surname != assessmentResult.Surname)
                    {
                        assessment.SetCandidateNames(assessmentResult.Id, modelResult.Surname, modelResult.Forenames);
                    }

                    if (modelResult.Result != assessmentResult.Result)
                    {
                        lastSelectedResult = model.Results.IndexOf(modelResult);
                        assessment.SetCandidateResult(assessmentResult.Id, modelResult.Result);
                    }
                }
            }

            // Check for new row
            if (!string.IsNullOrWhiteSpace(model.NewRow.Surname) && !string.IsNullOrWhiteSpace(model.NewRow.Forenames))
            {
                var newCandidateId = assessment.AddCandidate(model.NewRow.Surname, model.NewRow.Forenames);
                if (model.NewRow.Result.HasValue)
                {
                    assessment.SetCandidateResult(newCandidateId, model.NewRow.Result.Value);
                }
            }

            return this.RedirectToAction("Edit", new { id, lastSelectedResult });
        }

        // GET: /Assessment/DeleteResultRow/{id}?rowId={rowId}
        [HttpGet]
        public ActionResult DeleteResultRow(Guid id, Guid rowId)
        {
            var assessment = this.assessmentsRepo.Open(id);
            assessment.RemoveResult(rowId);

            return this.RedirectToAction("Edit", new { id });
        }
    }
}
