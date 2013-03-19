namespace AssessmentAnywhere.Models.AssessmentEditor
{
    using System;
    using System.Web.Mvc;

    public class UpdateResultRow
    {
        public Guid RowId { get; set; }

        public string Surname { get; set; }

        public string Forenames { get; set; }

        public decimal? Result { get; set; }

        public void Validate(ModelStateDictionary modelState, string prefix, decimal? totalMarks)
        {
            if (string.IsNullOrWhiteSpace(this.Surname))
            {
                modelState.AddModelError(prefix + ".Surname", "Surname cannot be blank.");
            }

            if (string.IsNullOrWhiteSpace(this.Forenames))
            {
                modelState.AddModelError(prefix + ".Forenames", "Forenames cannot be blank.");
            }

            if (!this.Result.HasValue)
            {
                return;
            }

            if (this.Result < 0)
            {
                modelState.AddModelError(prefix + ".Result", "Result must be a positive number.");
            }

            if (totalMarks.HasValue && this.Result > totalMarks)
            {
                modelState.AddModelError(prefix + ".Result", "Result cannot be greater than the total marks.");
            }
        }
    }
}
