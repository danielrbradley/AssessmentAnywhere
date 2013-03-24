namespace AssessmentAnywhere.Models.Api.Assessments
{
    using System.Collections.Generic;

    public class GetModel
    {
        public List<Result> Results { get; set; }

        public int TotalCount { get; set; }
    }
}