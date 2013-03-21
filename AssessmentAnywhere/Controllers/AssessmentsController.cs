namespace AssessmentAnywhere.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using AssessmentAnywhere.Models.Assessments;
    using AssessmentAnywhere.Services.Assessments;
    using AssessmentAnywhere.Services.GradeBoundaries;

    using Assessment = AssessmentAnywhere.Models.Assessments.Assessment;

    [Authorize]
    public class AssessmentsController : Controller
    {
        private readonly IAssessmentsRepo assessmentsRepo;

        private readonly GradeBoundariesRepo gradeBoundariesRepo;

        public AssessmentsController()
            : this(new AssessmentsRepo(), new GradeBoundariesRepo())
        {
        }

        public AssessmentsController(IAssessmentsRepo assessmentsRepo, GradeBoundariesRepo gradeBoundariesRepo)
        {
            this.assessmentsRepo = assessmentsRepo;
            this.gradeBoundariesRepo = gradeBoundariesRepo;
        }

        // GET: /Assessments/
        public ActionResult Index()
        {
            var assessments = this.assessmentsRepo.QueryAssessments().Select(a => new Assessment(a));
            var model = new IndexModel(assessments);
            return this.View(model);
        }

        // GET: /Assessment/Create
        public ActionResult Create()
        {
            var model = new CreateModel();
            return this.View(model);
        }

        // POST: /Assessment/Create
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

            return this.RedirectToAction("Index", "Assessments");
        }

        // GET/POST: /Assessment/Delete/{id}
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
                return this.RedirectToAction("Index", "Assessments");
            }

            return this.HttpNotFound();
        }
    }
}
