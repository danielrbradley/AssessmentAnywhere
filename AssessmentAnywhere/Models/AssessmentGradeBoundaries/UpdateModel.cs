namespace AssessmentAnywhere.Models.AssessmentGradeBoundaries
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class UpdateModel
    {
        public UpdateModel()
        {
            Boundaries = new List<UpdatedGradeBoundary>();
            NewBoundary = new UpdatedGradeBoundary();
        }

        [Range(double.Epsilon, double.MaxValue, ErrorMessage = "Total marks must be greater than zero.")]
        public decimal? TotalMarks { get; set; }

        public IList<UpdatedGradeBoundary> Boundaries { get; set; }

        public UpdatedGradeBoundary NewBoundary { get; set; }

        public bool HasNewBoundary
        {
            get
            {
                return this.NewBoundary != null && NewBoundary.IsSet;
            }
        }

        public void Validate(ModelStateDictionary modelState)
        {
            if (TotalMarks.HasValue && TotalMarks < 0)
            {
                modelState.AddModelError("TotalMarks", "Total marks must be a positive number");
            }

            if (HasNewBoundary)
            {
                NewBoundary.Validate(modelState, "NewBoundary", TotalMarks);
            }

            for (int i = 0; i < Boundaries.Count; i++)
            {
                var prefix = string.Format("Boundaries[{0}]", i);
                Boundaries[i].Validate(modelState, prefix, TotalMarks);
            }
        }
    }
}