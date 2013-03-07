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
        //
        // GET: /Assessments/

        public ActionResult Index()
        {
            var model = new IndexModel { Assessments = GetAssessmentsForIndex() };
            return View(model);
        }

        private List<Assessment> GetAssessmentsForIndex()
        {
            var repo = new AssessmentsRepo();
            return repo.QueryAssessments().Select(a => new Assessment { Id = a.Id, Name = a.Name }).ToList();
        }

        public ActionResult Create(Guid? registerId)
        {
            var model = new CreateModel
                            {
                                Name = string.Empty,
                                SelectedSubject = string.Empty,
                                AvailableSubjects = GetSubjectsForCreate(),
                            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(string name, string subject)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                var model = new CreateModel
                {
                    Name = name,
                    SelectedSubject = subject,
                    AvailableSubjects = GetSubjectsForCreate(),
                };
                return View(model);
            }

            var assessmentsRepo = new AssessmentsRepo();
            var assessment = assessmentsRepo.Create();
            assessment.SetName(name);

            if (!string.IsNullOrWhiteSpace(subject))
            {
                new SubjectRepo().AddSubjectAssessment(assessment.Id, subject);
            }

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
                                AllAssessments = GetAssessmentsForIndex(),
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
