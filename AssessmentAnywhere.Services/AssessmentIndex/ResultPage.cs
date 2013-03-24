namespace AssessmentAnywhere.Services.AssessmentIndex
{
    using System.Collections.Generic;

    internal class ResultPage : IResultPage
    {
        private readonly IEnumerable<IAssessment> results;

        private readonly int totalCount;

        public ResultPage(IEnumerable<IAssessment> results, int totalCount)
        {
            this.results = results;
            this.totalCount = totalCount;
        }

        public IEnumerable<IAssessment> Results
        {
            get
            {
                return this.results;
            }
        }

        public int TotalCount
        {
            get
            {
                return this.totalCount;
            }
        }
    }
}