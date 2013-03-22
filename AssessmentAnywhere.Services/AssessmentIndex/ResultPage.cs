namespace AssessmentAnywhere.Services.AssessmentIndex
{
    using System.Collections;
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

        public IEnumerator<IAssessment> GetEnumerator()
        {
            return this.results.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
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