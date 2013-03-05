using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
                                ExistingRegisters = GetRegistersForCreate()
                            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(string name, Guid? registerId)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                var model = new CreateModel
                {
                    Name = name,
                    SelectedRegisterId = registerId,
                    ExistingRegisters = GetRegistersForCreate()
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

            return RedirectToAction("Details", new { id = assessment.Id });
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
                                HasGrades = assessment.Boundaries.Any(),
                                AllAssessments = GetAssessmentsForIndex(),
                            };
            return View(model);
        }
    }
}
