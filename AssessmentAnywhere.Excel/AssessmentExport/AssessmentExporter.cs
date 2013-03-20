namespace AssessmentAnywhere.Excel.AssessmentExport
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using OfficeOpenXml;
    using OfficeOpenXml.Style;

    public static class AssessmentExporter
    {
        public static Stream Export(Assessment assessment)
        {
            var package = new ExcelPackage();
            var percentageStyle = package.Workbook.Styles.CreateNamedStyle("Percent");
            percentageStyle.BuildInId = 5;
            percentageStyle.Style.Numberformat.Format = "0%";

            var resultsWorksheet = package.Workbook.Worksheets.Add("Results");

            resultsWorksheet.SetValue(1, 1, "Surname");
            resultsWorksheet.SetValue(1, 2, "Forenames");
            resultsWorksheet.SetValue(1, 3, "Result");

            for (int i = 0; i < assessment.Rows.Count; i++)
            {
                var row = assessment.Rows[i];
                var rowNum = i + 2;

                resultsWorksheet.SetValue(rowNum, 1, row.Surname);
                resultsWorksheet.SetValue(rowNum, 2, row.Forenames);
                if (row.Result.HasValue)
                {
                    resultsWorksheet.SetValue(rowNum, 3, row.Result);
                }
            }

            int lastResultsColumn = 3;

            // Has percentages?
            if (assessment.TotalMarks.HasValue)
            {
                var resultsPercentageColumn = ++lastResultsColumn;

                // Definitions worksheet
                var definitionsWorksheet = package.Workbook.Worksheets.Add("Definitions");
                definitionsWorksheet.SetValue(1, 1, "Total Marks");
                definitionsWorksheet.SetValue(1, 2, assessment.TotalMarks);

                var totalMarksAddress = ExcelCellBase.GetFullAddress("Definitions", ExcelCellBase.GetAddress(1, 2, true));

                // Create percentage column
                resultsWorksheet.SetValue(1, resultsPercentageColumn, "Percentage");
                var percentageCellsAddress = ExcelCellBase.GetAddress(
                    2, resultsPercentageColumn, resultsWorksheet.Dimension.End.Row, resultsPercentageColumn);
                var percentageCells = resultsWorksheet.Cells[percentageCellsAddress];
                percentageCells.StyleName = "Percent";

                // Add percentage cells
                for (int i = 0; i < assessment.Rows.Count; i++)
                {
                    var rowNum = i + 2;
                    var formula = string.Format("=C{0}/({1})", rowNum, totalMarksAddress);
                    resultsWorksheet.Cells[rowNum, resultsPercentageColumn].Formula = formula;
                }
            }

            // Has grades?
            if (assessment.GradeBoundaries.Any())
            {
                var boundaries = assessment.GradeBoundaries.OrderBy(b => b.MinResult).ToList();
                var gradeColumn = ++lastResultsColumn;

                // Add worksheet
                var gradeBoundariesWorksheet = package.Workbook.Worksheets.Add("Grade Boundaries");
                gradeBoundariesWorksheet.SetValue(1, 1, "Minimum Mark");
                gradeBoundariesWorksheet.SetValue(1, 2, "Grade");

                for (int i = 0; i < boundaries.Count; i++)
                {
                    var gradeBoundary = boundaries[i];
                    var rowNum = i + 2;
                    gradeBoundariesWorksheet.SetValue(rowNum, 1, gradeBoundary.MinResult);
                    gradeBoundariesWorksheet.SetValue(rowNum, 2, gradeBoundary.Grade);
                }

                resultsWorksheet.SetValue(1, gradeColumn, "Grade");
                var gradeBoundariesRange = ExcelCellBase.GetAddress(2, 1, boundaries.Count + 1, 2, true);
                var gradeBoundariesAddress = ExcelCellBase.GetFullAddress("Grade Boundaries", gradeBoundariesRange);

                for (int i = 0; i < assessment.Rows.Count; i++)
                {
                    var rowNum = i + 2;
                    var formula = string.Format("=VLOOKUP(C{0},{1},2,TRUE)", rowNum, gradeBoundariesAddress);
                    resultsWorksheet.Cells[rowNum, gradeColumn].Formula = formula;
                }
            }

            package.Save();
            return package.Stream;
        }
    }
}
