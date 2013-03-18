namespace AssessmentAnywhere.Excel
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    using OfficeOpenXml;

    public static class AssessmentParser
    {
        public static ParseResult Parse(
            Stream excelFile,
            int worksheetPosition,
            string surnameCol,
            string forenamesCol,
            string resultCol,
            int startRow)
        {
            return Parse(
                excelFile,
                worksheetPosition,
                ColToInt(surnameCol),
                ColToInt(forenamesCol),
                ColToInt(resultCol),
                startRow);
        }

        public static int ColToInt(string columnRef)
        {
            if (string.IsNullOrEmpty(columnRef))
            {
                throw new ArgumentNullException("columnRef");
            }

            if (!Regex.IsMatch(columnRef, @"^[A-Z]+$"))
            {
                throw new FormatException("Column reference not valid");
            }

            return ColToIntRec(columnRef);
        }

        private static int ColToIntRec(string remaining)
        {
            if (!remaining.Any())
            {
                return 0;
            }

            var c = remaining.Last();
            var val = Convert.ToInt32(c) - 64;

            return val + (ColToIntRec(remaining.Substring(0, remaining.Length - 1)) * 26);
        }

        public static ParseResult Parse(Stream excelFile, int worksheetPosition, int surnameCol, int forenamesCol, int resultCol, int startRow)
        {
            using (var xlPackage = new ExcelPackage(excelFile))
            {
                var worksheet = xlPackage.Workbook.Worksheets[worksheetPosition];

                var results = new List<ResultRow>();
                var lastRow = worksheet.Cells.End.Row;
                var row = startRow;
                while (row < lastRow)
                {
                    var surname = worksheet.GetValue<string>(row, surnameCol);
                    var forenames = worksheet.GetValue<string>(row, forenamesCol);
                    var result = worksheet.GetValue<decimal?>(row, resultCol);

                    if (string.IsNullOrWhiteSpace(surname) && string.IsNullOrWhiteSpace(forenames))
                    {
                        break;
                    }

                    results.Add(new ResultRow(surname, forenames, result));

                    row++;
                }

                return new ParseResult(results);
            }
        }
    }
}
