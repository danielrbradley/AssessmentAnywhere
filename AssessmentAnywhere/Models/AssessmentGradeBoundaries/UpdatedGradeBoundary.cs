namespace AssessmentAnywhere.Models.AssessmentGradeBoundaries
{
    using System.Web.Mvc;

    public class UpdatedGradeBoundary
    {
        public bool IsSet
        {
            get
            {
                return !string.IsNullOrEmpty(Grade) || MinResult.HasValue;
            }
        }

        public string Grade { get; set; }

        public decimal? MinResult { get; set; }

        public void Validate(ModelStateDictionary modelState, string modelPrefix, decimal? totalMarks)
        {
            if (!this.IsSet)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(this.Grade))
            {
                modelState.AddModelError(modelPrefix + ".Grade", "Grade is required.");
            }

            if (!this.MinResult.HasValue)
            {
                modelState.AddModelError(modelPrefix + ".MinResult", "Minium required result is required.");
            }
            else
            {
                if (this.MinResult < 0)
                {
                    modelState.AddModelError(modelPrefix + ".MinResult", "Min result cannot be negative.");
                }

                if (totalMarks.HasValue && this.MinResult > totalMarks)
                {
                    modelState.AddModelError(modelPrefix + ".MinResult", "Min result must be less than total marks.");
                }
            }
        }
    }
}