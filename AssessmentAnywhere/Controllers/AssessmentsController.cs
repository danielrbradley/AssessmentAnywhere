using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AssessmentAnywhere.Controllers
{
    using System.Collections;

    using AssessmentAnywhere.Models.Assessments;
    using AssessmentAnywhere.Services;
    using AssessmentAnywhere.Services.Repos;
    using AssessmentAnywhere.Services.Repos.Models;

    using Assessment = AssessmentAnywhere.Models.Assessments.Assessment;

    public class AssessmentsController : Controller
    {
        private static readonly AssessmentsRepo assessmentsRepo = new AssessmentsRepo();

        private static readonly GradeBoundariesRepo gradeBoundariesRepo = new GradeBoundariesRepo();

        // GET: /Assessments/
        public ActionResult Index()
        {
            if (assessmentsRepo.QueryAssessments().Any())
            {
                return View(new IndexModel(assessmentsRepo.QueryAssessments().Select(a => new Assessment(a))));
            }

            return View("CreateFirst", new CreateModel());
        }

        [HttpPost]
        public ActionResult CreateFirst(CreateModel model)
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

            return RedirectToAction("Index");
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

        public ActionResult Details(Guid id)
        {
            var assessment = assessmentsRepo.Open(id);
            var model = new DetailsModel(assessment);
            return View(model);
        }

        public ActionResult Results(Guid id)
        {
            var assessment = assessmentsRepo.Open(id);
            bool hasBoundaries;
            var boundaries = gradeBoundariesRepo.TryOpen(id, out hasBoundaries);
            IEnumerable<ResultRow> model;

            if (hasBoundaries)
                model = GenerateResultsFromRepoModels(assessment, boundaries);
            else
                model = GenerateResultsFromRepoModels(assessment);

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        private static IEnumerable<ResultRow> GenerateResultsFromRepoModels(
            Services.Repos.Models.Assessment assessment, GradeBoundaries boundaries)
        {
            var results = from result in assessment.Results
                          let grade = boundaries.Boundaries.ForResult(result.Result)
                          let percentage = result.Result / boundaries.MaxResult
                          select new ResultRow(result.Id, result.CandidateName, result.Result, percentage, grade);

            return results.ToList();
        }

        private static IEnumerable<ResultRow> GenerateResultsFromRepoModels(Services.Repos.Models.Assessment assessment)
        {
            var results = from result in assessment.Results
                          select new ResultRow(result.Id, result.CandidateName, result.Result);
            return results.ToList();
        }

        [HttpPost]
        public ActionResult AddResult(Guid id, AddResultRowModel postbackModel)
        {
            var assessment = new AssessmentsRepo().Open(id);
            var rowId = assessment.AddCandidate(postbackModel.CandidateName);
            assessment.SetCandidateResult(rowId, postbackModel.Result);
            var returnModel = new ResultRow(rowId, postbackModel.CandidateName, postbackModel.Result);
            return Json(returnModel);
        }

        [HttpPost]
        public void DeleteResult(Guid id, DeleteResultRowModel postbackModel)
        {
            var assessment = new AssessmentsRepo().Open(id);
            assessment.RemoveResult(postbackModel.RowId);
        }

        [HttpPost]
        public void UpdateResult(Guid id, UpdateResultRowModel postbackModel)
        {
            var assessment = new AssessmentsRepo().Open(id);
            assessment.SetCandidateName(postbackModel.RowId, postbackModel.CandidateName);
            assessment.SetCandidateResult(postbackModel.RowId, postbackModel.Result);
        }

        //public ActionResult AddGradeBoundaries(Guid id)
        //{
        //    var gradeBoundariesRepo = new GradeBoundariesRepo();
        //    var boundary = gradeBoundariesRepo.OpenOrCreate(id);

        //    var boundaryList = new List<Boundary>
        //        {
        //            new Boundary {Grade = "A*", MinResult = 80},
        //            new Boundary {Grade = "A", MinResult = 70},
        //            new Boundary {Grade = "B", MinResult = 60},
        //            new Boundary {Grade = "C", MinResult = 50},
        //            new Boundary {Grade = "D", MinResult = 40},
        //            new Boundary {Grade = "E", MinResult = 30},
        //        };
        //    boundary.Boundaries = boundaryList;




        //    return RedirectToAction("Details", new { id = id });
        //}
    }
}
