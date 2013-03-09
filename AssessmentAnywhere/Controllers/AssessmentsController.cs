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
            var assessments = assessmentsRepo.QueryAssessments().Select(a => new Assessment(a));
            var model = new IndexModel(assessments);
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

        [HttpGet]
        public ActionResult Details(Guid id, int? lastSelectedResult)
        {
            var assessment = assessmentsRepo.Open(id);
            bool hasBoundaries;
            var boundaries = gradeBoundariesRepo.TryOpen(id, out hasBoundaries);
            DetailsModel model;

            if (hasBoundaries)
            {
                model = new DetailsModel(assessment, boundaries, lastSelectedResult);
            }
            else
            {
                model = new DetailsModel(assessment, lastSelectedResult);
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Update(Guid id, UpdateModel model)
        {
            var assessment = assessmentsRepo.Open(id);
            bool hasBoundaries;
            var boundaries = gradeBoundariesRepo.TryOpen(id, out hasBoundaries);

            if (model.Name != assessment.Name)
            {
                assessment.SetName(model.Name);
            }

            int? lastSelectedResult = null;

            // Check for updates
            foreach (var modelResult in model.Results.Where(r => r.RowId != Guid.Empty))
            {
                var assessmentResult = assessment.Results.Single(r => r.Id == modelResult.RowId);

                if (modelResult.CandidateName != assessmentResult.CandidateName)
                {
                    assessment.SetCandidateName(assessmentResult.Id, modelResult.CandidateName);
                }

                if (modelResult.Result != assessmentResult.Result)
                {
                    lastSelectedResult = model.Results.IndexOf(modelResult);
                    assessment.SetCandidateResult(assessmentResult.Id, modelResult.Result);
                }
            }

            // Check for new row
            if (model.Results.Last().RowId == Guid.Empty && !string.IsNullOrWhiteSpace(model.Results.Last().CandidateName))
            {
                assessment.AddCandidate(model.Results.Last().CandidateName);
            }

            return RedirectToAction("Details", new { id, lastSelectedResult });
        }

        public ActionResult DeleteResultRow(Guid id, Guid rowId)
        {
            var assessment = assessmentsRepo.Open(id);
            assessment.RemoveResult(rowId);

            return RedirectToAction("Details", new { id });
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
        public void UpdateResult(Guid id, UpdateResultRow postbackModel)
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
