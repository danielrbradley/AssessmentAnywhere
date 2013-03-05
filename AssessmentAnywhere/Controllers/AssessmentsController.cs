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

        private List<IndexModel.Assessment> GetAssessmentsForIndex()
        {
            return new List<IndexModel.Assessment>
                       {
                           new IndexModel.Assessment
                               {
                                   Id = new Guid(),
                                   Name = "Assessment 1"
                               }
                       };
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
            var newId = Guid.NewGuid();

            if (string.IsNullOrWhiteSpace(name) || !registerId.HasValue)
            {
                var model = new CreateModel
                {
                    Name = name,
                    SelectedRegisterId = registerId,
                    ExistingRegisters = GetRegistersForCreate()
                };
                return View(model);
            }

            var assessmentsRepo = new AssessmentsRepo();
            assessmentsRepo.Create(registerId.Value);

            return RedirectToAction("Details", new { id = newId });
        }

        private List<CreateModel.Register> GetRegistersForCreate()
        {
            return new List<CreateModel.Register> { new CreateModel.Register { Id = new Guid("e4b82e15-4820-4bde-8d93-d51382ff5939"), Name = "Test Register" } };
        }

        public ActionResult Details(Guid id)
        {
            return View();
        }
    }
}
