namespace AssessmentAnywhere.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using AssessmentAnywhere.Models.AssessmentGradeBoundaries;
    using AssessmentAnywhere.Services.Assessments;
    using AssessmentAnywhere.Services.GradeBoundaries;

    [Authorize]
    public class AssessmentGradeBoundariesController : Controller
    {
        private readonly IAssessmentsRepo assessmentsRepo;

        private readonly GradeBoundariesRepo gradeBoundariesRepo;

        public AssessmentGradeBoundariesController()
            : this(new AssessmentsRepo(), new GradeBoundariesRepo())
        {
        }

        public AssessmentGradeBoundariesController(IAssessmentsRepo assessmentsRepo, GradeBoundariesRepo gradeBoundariesRepo)
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
            // Validate
            model.Validate(this.ModelState);

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

            // ReSharper disable PossibleInvalidOperationException
            var boundariesToSave = model.Boundaries.Select(b => new Boundary(b.Grade, b.MinResult.Value)).ToList();
            if (model.HasNewBoundary)
            {
                boundariesToSave.Add(new Boundary(model.NewBoundary.Grade, model.NewBoundary.MinResult.Value));
            }
            // ReSharper restore PossibleInvalidOperationException

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

            boundaries.SetBoundaries(boundariesToSave);

            return this.RedirectToAction("Edit", new { id });
        }
    }
}
