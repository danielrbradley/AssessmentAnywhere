namespace AssessmentAnywhere.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using AssessmentAnywhere.Models.Assessments;
    using AssessmentAnywhere.Services.Repos;

    using Assessment = AssessmentAnywhere.Models.Assessments.Assessment;

    public class AssessmentsController : Controller
    {
        private readonly AssessmentsRepo assessmentsRepo = new AssessmentsRepo();

        private readonly GradeBoundariesRepo gradeBoundariesRepo = new GradeBoundariesRepo();

        // GET: /Assessments/
        public ActionResult Index()
        {
            var assessments = this.assessmentsRepo.QueryAssessments().Select(a => new Assessment(a));
            var model = new IndexModel(assessments);
            return this.View(model);
        }

        // GET: /Assessments/Create?registerId={registerId}
        public ActionResult Create(Guid? registerId)
        {
            var model = new CreateModel();
            return this.View(model);
        }

        [HttpPost]
        public ActionResult Create(CreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            if (this.assessmentsRepo.QueryAssessments().Any(a => a.Name == model.Name))
            {
                ModelState.AddModelError("Name", "You've already got another assessment with this name.");
                return this.View(model);
            }

            var assessment = this.assessmentsRepo.Create();
            assessment.SetName(model.Name);

            return this.RedirectToAction("Index");
        }

        public ActionResult Delete(Guid id)
        {
            if (this.Request.HttpMethod.Equals("GET", StringComparison.InvariantCultureIgnoreCase))
            {
                var assessment = this.assessmentsRepo.Open(id);
                var model = new DeleteModel(assessment);
                return this.View(model);
            }

            if (this.Request.HttpMethod.Equals("POST", StringComparison.InvariantCultureIgnoreCase))
            {
                this.assessmentsRepo.Delete(id);
                return this.RedirectToAction("Index");
            }

            return this.HttpNotFound();
        }

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

        [HttpPost]
        public ActionResult Update(Guid id, UpdateModel model)
        {
            var assessment = this.assessmentsRepo.Open(id);

            // Validate
            if (!string.IsNullOrWhiteSpace(model.NewRow.Surname)
                || !string.IsNullOrWhiteSpace(model.NewRow.Forenames))
            {
                if (string.IsNullOrWhiteSpace(model.NewRow.Surname))
                {
                    ModelState.AddModelError("NewRow.Surname", "Surname cannot be blank");
                }
                else if (string.IsNullOrWhiteSpace(model.NewRow.Forenames))
                {
                    ModelState.AddModelError("NewRow.Forenames", "Forenames cannot be blank");
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
                foreach (var modelResult in model.Results.Where(r => r.RowId != Guid.Empty))
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
                assessment.AddCandidate(model.NewRow.Surname, model.NewRow.Forenames);
            }

            return this.RedirectToAction("Edit", new { id, lastSelectedResult });
        }

        public ActionResult DeleteResultRow(Guid id, Guid rowId)
        {
            var assessment = this.assessmentsRepo.Open(id);
            assessment.RemoveResult(rowId);

            return this.RedirectToAction("Edit", new { id });
        }
    }
}
