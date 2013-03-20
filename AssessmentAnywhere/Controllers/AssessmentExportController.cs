namespace AssessmentAnywhere.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using AssessmentAnywhere.Excel.AssessmentExport;
    using AssessmentAnywhere.Services.Repos;
    using AssessmentAnywhere.Services.Repos.Models;

    using Assessment = AssessmentAnywhere.Excel.AssessmentExport.Assessment;

    public class AssessmentExportController : Controller
    {
        private readonly AssessmentsRepo assessmentsRepo = new AssessmentsRepo();

        private readonly GradeBoundariesRepo gradeBoundariesRepo = new GradeBoundariesRepo();

        [HttpGet]
        public ActionResult Xlsx(Guid id)
        {
            var assessment = this.assessmentsRepo.Open(id);

            bool hasGradeBoundaries;
            var gradeBoundaries = this.gradeBoundariesRepo.TryOpen(id, out hasGradeBoundaries);

            var exportAssessment = hasGradeBoundaries ? ForExport(assessment, gradeBoundaries) : ForExport(assessment);
            var stream = AssessmentExporter.Export(exportAssessment);
            string downloadName = string.Concat(assessment.Name, ".xlsx");
            return this.File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", downloadName);
        }

        private static Assessment ForExport(Services.Repos.Models.Assessment assessment)
        {
            return new Assessment { TotalMarks = assessment.TotalMarks, Rows = ForExport(assessment.Results) };
        }

        private static IList<AssessmentRow> ForExport(IEnumerable<AssessmentResult> results)
        {
            return results.Select(r => new AssessmentRow { Surname = r.Surname, Forenames = r.Forenames, Result = r.Result }).ToList();
        }

        private static Assessment ForExport(Services.Repos.Models.Assessment assessment, Services.Repos.Models.GradeBoundaries gradeBoundaries)
        {
            return new Assessment { TotalMarks = assessment.TotalMarks, Rows = ForExport(assessment.Results), GradeBoundaries = ForExport(gradeBoundaries) };
        }

        private static IList<GradeBoundary> ForExport(GradeBoundaries gradeBoundaries)
        {
            return gradeBoundaries.Boundaries.Select(b => new GradeBoundary { Grade = b.Grade, MinResult = b.MinResult }).ToList();
        }
    }
}