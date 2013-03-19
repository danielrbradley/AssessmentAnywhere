namespace AssessmentAnywhere.Models.AssessmentEditor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class UpdateModel
    {
        public UpdateModel()
        {
            this.NewRow = new UpdateResultRow();
            this.Results = new List<UpdateResultRow>();
        }

        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public IList<UpdateResultRow> Results { get; set; }

        public UpdateResultRow NewRow { get; set; }

        [Range(double.Epsilon, double.MaxValue, ErrorMessage = "Total marks must be greater than zero.")]
        public decimal? TotalMarks { get; set; }

        public bool HasNewRow
        {
            get
            {
                return !string.IsNullOrEmpty(this.NewRow.Surname) || !string.IsNullOrEmpty(this.NewRow.Forenames)
                       || this.NewRow.Result.HasValue;
            }
        }

        public void Validate(ModelStateDictionary modelState)
        {
            if (this.HasNewRow)
            {
                this.NewRow.Validate(modelState, "NewRow", this.TotalMarks);
            }

            foreach (var result in this.Results)
            {
                var prefix = string.Format("Results[{0}]", this.Results.IndexOf(result));
                result.Validate(modelState, prefix, this.TotalMarks);
            }
        }
    }
}
