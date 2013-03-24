namespace AssessmentAnywhere.Services.AssessmentIndex
{
    using System.Collections.Generic;

    public interface IResultPage
    {
        IEnumerable<IAssessment> Results { get; }

        int TotalCount { get; }
    }
}