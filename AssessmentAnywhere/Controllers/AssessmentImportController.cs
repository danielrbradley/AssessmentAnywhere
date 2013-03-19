namespace AssessmentAnywhere.Controllers
{
    using System;
    using System.IO;
    using System.Web;
    using System.Web.Mvc;

    using AssessmentAnywhere.Excel;
    using AssessmentAnywhere.Models.AssessmentImport;
    using AssessmentAnywhere.Services.Repos;

    public class AssessmentImportController : Controller
    {
        private readonly AssessmentsRepo assessmentsRepo = new AssessmentsRepo();

        [HttpGet]
        public ActionResult Upload(Guid id)
        {
            var assessment = this.assessmentsRepo.Open(id);

            var model = new UploadViewModel(assessment);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Upload(Guid id, HttpPostedFileBase fileToImport, UploadPostbackModel model)
        {
            var assessment = this.assessmentsRepo.Open(id);

            if (fileToImport == null)
            {
                ModelState.AddModelError("fileToImport", "No file selected");
            }

            if (!ModelState.IsValid)
            {
                var viewModel = new UploadViewModel(assessment, model);
                return this.View(viewModel);
            }

            using (var memoryStream = new MemoryStream())
            {
                fileToImport.InputStream.CopyTo(memoryStream);
                memoryStream.Position = 0;
                var parseResult = AssessmentParser.Parse(
                    memoryStream,
                    model.WorksheetNumber,
                    model.SurnameColumn,
                    model.ForenamesColumn,
                    model.ResultColumn,
                    model.StartRow);

                foreach (var resultRow in parseResult.Results)
                {
                    var newCandidateId = assessment.AddCandidate(resultRow.Surname, resultRow.Forenames);
                    if (resultRow.Result.HasValue)
                    {
                        assessment.SetCandidateResult(newCandidateId, resultRow.Result.Value);
                    }
                }
            }

            return this.RedirectToAction("Edit", "AssessmentEditor", new { id });
        }
    }
}
