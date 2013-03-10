//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace AssessmentAnywhere.Controllers
//{
//    using AssessmentAnywhere.Models.GradeBoundaries;
//    using AssessmentAnywhere.Services.Repos;
//    using AssessmentAnywhere.Services.Repos.Models;

//    using EditModel = AssessmentAnywhere.Models.Assessments.EditModel;

//    public class GradeBoundariesController : Controller
//    {
//        public ActionResult Edit(Guid id)
//        {
//            var assessment = new AssessmentsRepo().Open(id);
//            var boundaries = new GradeBoundariesRepo().OpenOrCreate(id);
//            var model = new EditModel
//                            {
//                                AssessmentId = id,
//                                AssessmentName = assessment.Name,
//                                Boundaries = ConstructBoundaries(boundaries)
//                            };
//            return View(model);
//        }

//        private List<EditModel.Boundary> ConstructBoundaries(GradeBoundaries boundaries)
//        {
//            return boundaries.Boundaries.Select(b => new EditModel.Boundary { Grade = b.Grade, MinResult = b.MinResult }).ToList();
//        }

//        public ActionResult Add(Guid id, string grade, int minResult)
//        {
//            var boundaries = new GradeBoundariesRepo().OpenOrCreate(id);
//            boundaries.Boundaries.Add(new Boundary { Grade = grade, MinResult = minResult });

//            return RedirectToAction("Edit", new { id = id });
//        }

//        public ActionResult Update(Guid id, string grade, int minResult)
//        {
//            var boundaries = new GradeBoundariesRepo().OpenOrCreate(id);
//            boundaries.Boundaries.Single(b => b.Grade == grade).MinResult = minResult;

//            return RedirectToAction("Edit", new { id = id });
//        }

//        public ActionResult Remove(Guid id, string grade)
//        {
//            var boundaries = new GradeBoundariesRepo().OpenOrCreate(id);
//            boundaries.Boundaries.RemoveAll(b => b.Grade == grade);

//            return RedirectToAction("Edit", new { id = id });
//        }
//    }
//}
