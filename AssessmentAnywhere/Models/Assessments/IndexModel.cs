namespace AssessmentAnywhere.Models.Assessments
{
    using System.Collections.Generic;
    using System.Linq;

    public class IndexModel
    {
        public int Skip { get; private set; }

        public int Top { get; private set; }

        public IEnumerable<Assessment> Results { get; private set; }

        public int TotalCount { get; private set; }

        public IndexModel(int skip, int top, IEnumerable<Assessment> results, int totalCount)
        {
            Skip = skip;
            Top = top;
            Results = results;
            TotalCount = totalCount;
        }

        public IndexModel(int skip, int top, Services.AssessmentIndex.IResultPage resultPage)
            : this(skip, top, resultPage.Results.Select(r => new Assessment(r)).ToArray(), resultPage.TotalCount)
        {
        }
    }
}
