namespace AssessmentAnywhere.Models.AssessmentGradeBoundaries
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
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

                if (this.Boundaries.Any(b => b.Grade == NewBoundary.Grade))
                {
                    modelState.AddModelError("NewBoundary.Grade", "Boundary grade must be unique.");
                }

                if (this.Boundaries.Any(b => b.MinResult == NewBoundary.MinResult))
                {
                    modelState.AddModelError("NewBoundary.MinResult", "Boundary minimum required result must be unique.");
                }
            }

            for (int i = 0; i < Boundaries.Count; i++)
            {
                var prefix = string.Format("Boundaries[{0}]", i);
                Boundaries[i].Validate(modelState, prefix, TotalMarks);
            }

            var duplicateGrades = this.Boundaries.Where(b1 => this.Boundaries.Count(b2 => b2.Grade == b1.Grade) > 1);
            foreach (var duplicateGrade in duplicateGrades)
            {
                var i = this.Boundaries.IndexOf(duplicateGrade);
                var key = string.Format("Boundaries[{0}].Grade", i);
                modelState.AddModelError(key, "Boundary grade must be unique.");
            }

            var duplicateMinResults =
                this.Boundaries.Where(b1 => this.Boundaries.Count(b2 => b2.MinResult == b1.MinResult) > 1);
            foreach (var duplicateMinResult in duplicateMinResults)
            {
                var i = this.Boundaries.IndexOf(duplicateMinResult);
                var key = string.Format("Boundaries[{0}].MinResult", i);
                modelState.AddModelError(key, "Boundary minimum required result must be unique.");
            }
        }
    }
}