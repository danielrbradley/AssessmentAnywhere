namespace AssessmentAnywhere.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using AssessmentAnywhere.Excel.AssessmentExport;
    using AssessmentAnywhere.Services.Assessments;
    using AssessmentAnywhere.Services.GradeBoundaries;

    using Assessment = AssessmentAnywhere.Excel.AssessmentExport.Assessment;

    public class AssessmentExportController : Controller
    {
        private readonly IAssessmentsRepo assessmentsRepo;

        private readonly GradeBoundariesRepo gradeBoundariesRepo;

        public AssessmentExportController()
            : this(new AssessmentsRepo(), new GradeBoundariesRepo())
        {
        }

        public AssessmentExportController(IAssessmentsRepo assessmentsRepo, GradeBoundariesRepo gradeBoundariesRepo)
        {
            this.assessmentsRepo = assessmentsRepo;
            this.gradeBoundariesRepo = gradeBoundariesRepo;
        }

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

        private static Assessment ForExport(IAssessment assessment)
        {
            return new Assessment { TotalMarks = assessment.TotalMarks, Rows = ForExport(assessment.Results) };
        }

        private static IList<AssessmentRow> ForExport(IEnumerable<IAssessmentResult> results)
        {
            return results.Select(r => new AssessmentRow { Surname = r.Surname, Forenames = r.Forenames, Result = r.Result }).ToList();
        }

        private static Assessment ForExport(IAssessment assessment, IGradeBoundaries gradeBoundaries)
        {
            return new Assessment { TotalMarks = assessment.TotalMarks, Rows = ForExport(assessment.Results), GradeBoundaries = ForExport(gradeBoundaries) };
        }

        private static IList<GradeBoundary> ForExport(IGradeBoundaries gradeBoundaries)
        {
            return gradeBoundaries.Boundaries.Select(b => new GradeBoundary { Grade = b.Grade, MinResult = b.MinResult }).ToList();
        }
    }
}