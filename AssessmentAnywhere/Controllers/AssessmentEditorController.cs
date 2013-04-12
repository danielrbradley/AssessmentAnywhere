namespace AssessmentAnywhere.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using AssessmentAnywhere.Models.AssessmentEditor;
    using AssessmentAnywhere.Services.AssessmentIndex;
    using AssessmentAnywhere.Services.Assessments;
    using AssessmentAnywhere.Services.GradeBoundaries;

    [Authorize]
    public class AssessmentEditorController : Controller
    {
        private readonly IAssessmentsRepo assessmentsRepo;

        private readonly IGradeBoundariesRepo gradeBoundariesRepo;

        private readonly IAssessmentIndex assessmentIndex;

        public AssessmentEditorController(IAssessmentsRepo assessmentsRepo, IGradeBoundariesRepo gradeBoundariesRepo, IAssessmentIndex assessmentIndex)
        {
            this.assessmentsRepo = assessmentsRepo;
            this.gradeBoundariesRepo = gradeBoundariesRepo;
            this.assessmentIndex = assessmentIndex;
        }

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
            var assessment = this.assessmentsRepo.Open(id);

            model.Validate(this.ModelState);

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
                this.assessmentIndex.Set(id, model.Name);
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
