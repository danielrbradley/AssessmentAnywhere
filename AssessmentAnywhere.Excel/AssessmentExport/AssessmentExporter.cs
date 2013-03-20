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
            using (var package = new ExcelPackage())
            {
                // Create percent cell style
                var percentageStyle = package.Workbook.Styles.CreateNamedStyle("Percent");
                percentageStyle.BuildInId = 5;
                percentageStyle.Style.Numberformat.Format = "0%";

                // Create headings cell style
                var headingsStyle = package.Workbook.Styles.CreateNamedStyle("Heading");
                headingsStyle.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                headingsStyle.Style.Font.Bold = true;

                var resultsWorksheet = package.Workbook.Worksheets.Add("Results");

                resultsWorksheet.SetValue(1, 1, "Surname");
                resultsWorksheet.Column(1).Width = 15;
                resultsWorksheet.SetValue(1, 2, "Forenames");
                resultsWorksheet.Column(2).Width = 15;
                resultsWorksheet.SetValue(1, 3, "Result");
                resultsWorksheet.Column(3).Width = 7;

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
                    var percentageColumn = ++lastResultsColumn;

                    // Definitions worksheet
                    var definitionsWorksheet = package.Workbook.Worksheets.Add("Definitions");
                    definitionsWorksheet.Column(1).Width = 14;
                    definitionsWorksheet.SetValue(1, 1, "Total Marks");
                    definitionsWorksheet.SetValue(1, 2, assessment.TotalMarks);

                    var totalMarksAddress = ExcelCellBase.GetFullAddress(
                        "Definitions", ExcelCellBase.GetAddress(1, 2, true));

                    // Create percentage column
                    resultsWorksheet.SetValue(1, percentageColumn, "Percentage");
                    resultsWorksheet.Column(percentageColumn).Width = 11;

                    var percentageCellsAddress = ExcelCellBase.GetAddress(
                        2, percentageColumn, resultsWorksheet.Dimension.End.Row, percentageColumn);
                    var percentageCells = resultsWorksheet.Cells[percentageCellsAddress];
                    percentageCells.StyleName = "Percent";

                    // Add percentage cells
                    for (int i = 0; i < assessment.Rows.Count; i++)
                    {
                        var rowNum = i + 2;
                        var formula = string.Format("=C{0}/({1})", rowNum, totalMarksAddress);
                        resultsWorksheet.Cells[rowNum, percentageColumn].Formula = formula;
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
                    gradeBoundariesWorksheet.Column(1).Width = 15;
                    gradeBoundariesWorksheet.Cells[1, 1, 1, 2].StyleName = "Heading";

                    for (int i = 0; i < boundaries.Count; i++)
                    {
                        var gradeBoundary = boundaries[i];
                        var rowNum = i + 2;
                        gradeBoundariesWorksheet.SetValue(rowNum, 1, gradeBoundary.MinResult);
                        gradeBoundariesWorksheet.SetValue(rowNum, 2, gradeBoundary.Grade);
                    }

                    resultsWorksheet.SetValue(1, gradeColumn, "Grade");
                    resultsWorksheet.Column(gradeColumn).Width = 7;
                    var gradeBoundariesRange = ExcelCellBase.GetAddress(2, 1, boundaries.Count + 1, 2, true);
                    var gradeBoundariesAddress = ExcelCellBase.GetFullAddress("Grade Boundaries", gradeBoundariesRange);

                    for (int i = 0; i < assessment.Rows.Count; i++)
                    {
                        var rowNum = i + 2;
                        var formula = string.Format("=VLOOKUP(C{0},{1},2,TRUE)", rowNum, gradeBoundariesAddress);
                        resultsWorksheet.Cells[rowNum, gradeColumn].Formula = formula;
                    }
                }

                resultsWorksheet.Cells[1, 1, 1, lastResultsColumn].StyleName = "Heading";

                package.Save();
                package.Stream.Position = 0;
                return package.Stream;
            }
        }
    }
}
