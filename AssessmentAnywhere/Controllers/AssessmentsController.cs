namespace AssessmentAnywhere.Controllers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;

    using AssessmentAnywhere.Models.Assessments;
    using AssessmentAnywhere.Services.AssessmentIndex;
    using AssessmentAnywhere.Services.Assessments;

    [Authorize]
    public class AssessmentsController : Controller
    {
        private readonly IAssessmentIndex assessmentIndex;

        private readonly IAssessmentsRepo assessmentsRepo;

        public AssessmentsController(IAssessmentsRepo assessmentsRepo, IAssessmentIndex assessmentIndex)
        {
            this.assessmentsRepo = assessmentsRepo;
            this.assessmentIndex = assessmentIndex;
        }

        // GET: /Assessments/
        [SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1503:CurlyBracketsMustNotBeOmitted", Justification = "Reviewed. Only used for parameter guard cases.")]
        public ActionResult Index(int skip = 0, int top = 100)
        {
            if (skip < 0) skip = 0;
            if (top < 0) top = 0;
            if (top > 100) top = 100;

            var searchResultPage = this.assessmentIndex.Search(skip, top);

            var model = new IndexModel(skip, top, searchResultPage);
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

            if (this.assessmentIndex.ContainsName(model.Name))
            {
                ModelState.AddModelError("Name", "You've already got another assessment with this name.");
                return this.View(model);
            }

            var assessment = this.assessmentsRepo.Create();
            assessment.SetName(model.Name);
            this.assessmentIndex.Set(assessment.Id, assessment.Name);

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
                this.assessmentIndex.Delete(id);
                this.assessmentsRepo.Delete(id);
                return this.RedirectToAction("Index", "Assessments");
            }

            return this.HttpNotFound();
        }
    }
}
