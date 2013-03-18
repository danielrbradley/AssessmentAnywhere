namespace AssessmentAnywhere.Excel
{
    using System.Collections.Generic;

    public class ParseResult
    {
        private readonly IEnumerable<ResultRow> results;

        public ParseResult(IEnumerable<ResultRow> results)
        {
            this.results = results;
        }

        public IEnumerable<ResultRow> Results
        {
            get
            {
                return this.results;
            }
        }
    }
}