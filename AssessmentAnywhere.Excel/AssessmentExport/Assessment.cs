namespace AssessmentAnywhere.Excel.AssessmentExport
{
    using System.Collections.Generic;

    public class Assessment
    {
        public Assessment()
        {
            Rows = new List<AssessmentRow>();
            GradeBoundaries = new List<GradeBoundary>();
        }

        public decimal? TotalMarks { get; set; }

        public IList<AssessmentRow> Rows { get; set; }

        public IList<GradeBoundary> GradeBoundaries { get; set; }
    }
}
