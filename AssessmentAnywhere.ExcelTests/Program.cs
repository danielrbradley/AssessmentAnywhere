namespace AssessmentAnywhere.ExcelTests
{
    using System.IO;

    public class Program
    {
        public static void Main(string[] args)
        {
            using (var stream = File.OpenRead("Test.xlsx"))
            {
                var result = AssessmentAnywhere.Excel.AssessmentParser.Parse(stream, 1, "A", "B", "C", 1);
            }
        }
    }
}
