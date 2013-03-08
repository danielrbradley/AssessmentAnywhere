using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AssessmentAnywhere.Controllers
{
    using AssessmentAnywhere.Models.Assessments;
    using AssessmentAnywhere.Services;
    using AssessmentAnywhere.Services.Repos;
    using AssessmentAnywhere.Services.Repos.Models;

    using Assessment = AssessmentAnywhere.Models.Assessments.Assessment;

    public class AssessmentsController : Controller
    {
        private static readonly AssessmentsRepo assessmentsRepo = new AssessmentsRepo();

        // GET: /Assessments/
        public ActionResult Index()
        {
            var model = new IndexModel(assessmentsRepo.QueryAssessments().Select(a => new Assessment(a)));
            return View(model);
        }

        // GET: /Assessments/Create?registerId={registerId}
        public ActionResult Create(Guid? registerId)
        {
            var model = new CreateModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (assessmentsRepo.QueryAssessments().Any(a => a.Name == model.Name))
            {
                ModelState.AddModelError("Name", "You've already got another assessment with this name.");
                return View(model);
            }

            var assessment = assessmentsRepo.Create();
            assessment.SetName(model.Name);

            return RedirectToAction("Details", new { id = assessment.Id });
        }

        private List<string> GetSubjectsForCreate()
        {
            return new SubjectRepo().GetSubjects();
        }

        public ActionResult Details(Guid id)
        {
            var assessment = new AssessmentService().GetAssessmentGrades(id);
            var model = new DetailsModel
                            {
                                Id = assessment.AssessmentId,
                                Name = assessment.AssessmentName,
                                Candidates = assessment.Candidates.Select(c => new DetailsModel.Candidate { Name = c.Name, Result = c.Result, Grade = c.Grade }).ToList(),
                                HasBoundaries = assessment.Boundaries.Any(),
                                AllAssessments = assessmentsRepo.QueryAssessments().Select(a => new Assessment(a)),
                            };
            return View(model);
        }

        [HttpPost]
        public void AddCandidate(Guid id, string name)
        {
            var assessment = new AssessmentsRepo().Open(id);
            assessment.AddCandidate(name);
        }

        [HttpPost]
        public ActionResult SetResult(Guid id, string candidateName, decimal? result)
        {
            var assessment = new AssessmentsRepo().Open(id);
            assessment.SetCandidateResult(candidateName, result);
            return RedirectToAction("Details", new { id = id });
        }

        public ActionResult AddGradeBoundaries(Guid id)
        {
            var gradeBoundariesRepo = new GradeBoundariesRepo();
            var boundary = gradeBoundariesRepo.OpenOrCreate(id);

            var boundaryList = new List<Boundary>
                {
                    new Boundary {Grade = "A*", MinResult = 80},
                    new Boundary {Grade = "A", MinResult = 70},
                    new Boundary {Grade = "B", MinResult = 60},
                    new Boundary {Grade = "C", MinResult = 50},
                    new Boundary {Grade = "D", MinResult = 40},
                    new Boundary {Grade = "E", MinResult = 30},
                };
            boundary.Boundaries = boundaryList;




            return RedirectToAction("Details", new {id = id});
        }
    }
}
