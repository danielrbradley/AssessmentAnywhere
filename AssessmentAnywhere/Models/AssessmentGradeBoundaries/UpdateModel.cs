namespace AssessmentAnywhere.Models.AssessmentGradeBoundaries
{
    using System.Collections.Generic;

    public class UpdateModel
    {
        public decimal? TotalMarks { get; set; }

        public IList<UpdatedGradeBoundary> Boundaries { get; set; }

        public UpdatedGradeBoundary NewBoundary { get; set; }
    }
}