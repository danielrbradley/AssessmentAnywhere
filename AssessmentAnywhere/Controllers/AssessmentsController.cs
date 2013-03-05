using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AssessmentAnywhere.Services.Model;

namespace AssessmentAnywhere.Controllers
{
    using AssessmentAnywhere.Models.Assessments;
    using AssessmentAnywhere.Services.Repos;

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
            return repo.GetAssessments().Select(a => new Assessment { Id = a.Id, Name = a.Name }).ToList();
        }

        public ActionResult Create(Guid? registerId)
        {
            var model = new CreateModel
                            {
                                Name = string.Empty,
                                SelectedRegisterId = registerId,
                                ExistingRegisters = GetRegistersForCreate(),
                                SelectedSubject = string.Empty,
                                AvailableSubjects = GetSubjectsForCreate(),
                            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(string name, Guid? registerId, string subject)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                var model = new CreateModel
                {
                    Name = name,
                    SelectedRegisterId = registerId,
                    ExistingRegisters = GetRegistersForCreate(),
                    SelectedSubject = subject,
                    AvailableSubjects = GetSubjectsForCreate(),
                };
                return View(model);
            }

            if (!registerId.HasValue)
            {
                var newRegister = new RegistersRepo().Create();
                newRegister.Name = string.Format("{0} register", name);
                registerId = newRegister.Id;
            }

            var assessmentsRepo = new AssessmentsRepo();
            var assessment = assessmentsRepo.Create(registerId.Value);
            assessment.Name = name;

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

        private List<CreateModel.Register> GetRegistersForCreate()
        {
            return new RegistersRepo().GetRegisters().Select(r => new CreateModel.Register { Id = r.Id, Name = r.Name }).ToList();
        }

        public ActionResult Details(Guid id)
        {
            var assessment = new Services.Services.AssessmentGradesService().GetAssessmentGrades(id);
            var model = new DetailsModel
                            {
                                Id = assessment.AsssessmentId,
                                Name = assessment.AssessmentName,
                                Candidates = assessment.Candidates.Select(c => new DetailsModel.Candidate { Id = c.Id, Name = c.Name, Result = c.Result, Grade = c.Grade }).ToList(),
                                HasBoundaries = assessment.Boundaries.Any(),
                                AllAssessments = GetAssessmentsForIndex(),
                            };
            return View(model);
        }

        [HttpPost]
        public void AddCandidate(Guid id, string name)
        {
            var assessment = new AssessmentsRepo().Open(id);
            var register = new RegistersRepo().Open(assessment.RegisterId);
            var candidateId = Guid.NewGuid();
            register.Candidates.Add(new Services.Model.Candidate { Id = candidateId, Name = name });
            assessment.Results.Add(new Services.Model.AssessmentResult { CandidateId = candidateId });
        }

        [HttpPost]
        public ActionResult SetResult(Guid id, Guid candidateId, decimal? result)
        {
            var assessment = new AssessmentsRepo().Open(id);
            var boundaries = new GradeBoundariesRepo().Open(id);
            assessment.Results.First(c => c.CandidateId == candidateId).Result = result;
            return RedirectToAction("Details", new { id = id });
        }

        public ActionResult AddGradeBoundaries(Guid id)
        {
            var gradeBoundariesRepo = new GradeBoundariesRepo();
            var boundary = gradeBoundariesRepo.Create(id);

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
