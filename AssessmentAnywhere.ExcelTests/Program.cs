namespace AssessmentAnywhere.ExcelTests
{
    using System.IO;

    using AssessmentAnywhere.Excel.AssessmentExport;

    public class Program
    {
        public static void Main(string[] args)
        {
            //using (var stream = File.OpenRead("Test.xlsx"))
            //{
            //    var result = AssessmentAnywhere.Excel.AssessmentParser.Parse(stream, 1, "A", "B", "C", 1);
            //}

            var assessment = new Assessment
                                 {
                                     Rows =
                                         {
                                             new AssessmentRow
                                                 {
                                                     Surname = "Bradley",
                                                     Forenames = "Daniel",
                                                     Result = 57m,
                                                 },
                                             new AssessmentRow
                                                 {
                                                     Surname = "Bradley",
                                                     Forenames = "Sarah",
                                                     Result = 71m,
                                                 },
                                         },
                                     TotalMarks = 100m,
                                     GradeBoundaries =
                                         {
                                             new GradeBoundary { Grade = "A", MinResult = 70 },
                                             new GradeBoundary { Grade = "B", MinResult = 60 },
                                             new GradeBoundary { Grade = "C", MinResult = 50 },
                                             new GradeBoundary { Grade = "D", MinResult = 40 },
                                             new GradeBoundary { Grade = "E", MinResult = 30 },
                                             new GradeBoundary { Grade = "F", MinResult = 0 },
                                         }
                                 };

            var exportStream = AssessmentExporter.Export(assessment);
            exportStream.Position = 0;

            using (var fileStream = File.Open("ExportTest.xlsx", FileMode.Create))
            {
                exportStream.CopyTo(fileStream);
                fileStream.Flush();
            }
        }
    }
}
