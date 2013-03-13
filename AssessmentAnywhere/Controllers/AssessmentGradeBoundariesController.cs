namespace AssessmentAnywhere.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using AssessmentAnywhere.Models.AssessmentGradeBoundaries;
    using AssessmentAnywhere.Services.Repos;

    public class AssessmentGradeBoundariesController : Controller
    {
        private readonly AssessmentsRepo assessmentsRepo = new AssessmentsRepo();

        private readonly GradeBoundariesRepo gradeBoundariesRepo = new GradeBoundariesRepo();

        public AssessmentGradeBoundariesController()
            : this(new AssessmentsRepo(), new GradeBoundariesRepo())
        {
        }

        public AssessmentGradeBoundariesController(AssessmentsRepo assessmentsRepo, GradeBoundariesRepo gradeBoundariesRepo)
        {
            this.assessmentsRepo = assessmentsRepo;
            this.gradeBoundariesRepo = gradeBoundariesRepo;
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            var assessment = this.assessmentsRepo.Open(id);
            bool hasBoundaries;
            var boundaries = this.gradeBoundariesRepo.TryOpen(id, out hasBoundaries);

            var model = hasBoundaries ? new EditModel(assessment, boundaries.Boundaries) : new EditModel(assessment);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Update(Guid id, UpdateModel model)
        {
            if (model.Boundaries == null)
            {
                model.Boundaries = new List<UpdatedGradeBoundary>();
            }

            if (model.TotalMarks.HasValue)
            {
                if (model.TotalMarks.Value < 0)
                {
                    ModelState.AddModelError("TotalMarks", "Total marks must be a positive number");
                }

                var exceededBoundaries = model.Boundaries.Where(b => b.MinResult > model.TotalMarks);
                foreach (var boundary in exceededBoundaries)
                {
                    ModelState.AddModelError(
                        string.Format("Boundaries[{0}].MinResult", model.Boundaries.IndexOf(boundary)), "Min result must be less than total marks.");
                }

                if (model.NewBoundary != null && model.NewBoundary.MinResult.HasValue
                    && model.NewBoundary.MinResult > model.TotalMarks)
                {
                    ModelState.AddModelError("NewBoundary.MinResult", "Min result must be less than total marks.");
                }
            }

            if (model.NewBoundary != null
                && !(string.IsNullOrWhiteSpace(model.NewBoundary.Grade) || !model.TotalMarks.HasValue))
            {
                if (string.IsNullOrWhiteSpace(model.NewBoundary.Grade))
                {
                    ModelState.AddModelError("NewBoundary.Grade", "Grade is required.");
                }
                else if (!model.NewBoundary.MinResult.HasValue)
                {
                    ModelState.AddModelError("NewBoundary.MinResult", "Minimum result is required.");
                }
            }

            foreach (var boundary in model.Boundaries.Where(b => !string.IsNullOrWhiteSpace(b.Grade) && !b.MinResult.HasValue))
            {
                ModelState.AddModelError(
                    string.Format("Boundaries[{0}].MinResult", model.Boundaries.IndexOf(boundary)), "Min result is required");
            }

            foreach (var boundary in model.Boundaries.Where(b => string.IsNullOrWhiteSpace(b.Grade) && b.MinResult.HasValue))
            {
                ModelState.AddModelError(
                    string.Format("Boundaries[{0}].Grade", model.Boundaries.IndexOf(boundary)), "Grade is required");
            }

            var assessment = this.assessmentsRepo.Open(id);

            if (!ModelState.IsValid)
            {
                var editBoundaries = model.Boundaries.Select(b => new GradeBoundary(b.Grade, b.MinResult)).ToList();
                var newBoundary = (model.NewBoundary == null)
                                      ? GradeBoundary.New
                                      : new GradeBoundary(model.NewBoundary.Grade, model.NewBoundary.MinResult);
                var viewModel = new EditModel(id, assessment.Name, model.TotalMarks, editBoundaries, newBoundary);

                return this.View("Edit", viewModel);
            }

            var boundariesToSave = model.Boundaries.Where(b => !string.IsNullOrWhiteSpace(b.Grade) && b.MinResult.HasValue).ToList();
            if (model.NewBoundary != null && !string.IsNullOrWhiteSpace(model.NewBoundary.Grade) && model.NewBoundary.MinResult.HasValue)
            {
                boundariesToSave.Add(model.NewBoundary);
            }

            // Write changes to repos.
            bool hasBoundaries;
            var boundaries = this.gradeBoundariesRepo.TryOpen(id, out hasBoundaries);

            if (!hasBoundaries)
            {
                boundaries = this.gradeBoundariesRepo.Create(id);
            }

            if (model.TotalMarks != assessment.TotalMarks)
            {
                assessment.SetTotalMarks(model.TotalMarks);
            }

            boundaries.SetBoundaries(
                boundariesToSave.Select(b => new Services.Repos.Models.Boundary(b.Grade, b.MinResult.Value)));

            return RedirectToAction("Edit", new { id = id });
        }
    }
}
