namespace AssessmentAnywhere.Models.Assessments
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class UpdateModel
    {
        public UpdateModel()
        {
            NewRow = new UpdateResultRow();
            Results = new List<UpdateResultRow>();
        }

        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public IList<UpdateResultRow> Results { get; set; }

        public UpdateResultRow NewRow { get; set; }

        [Range(double.Epsilon, double.MaxValue, ErrorMessage = "Total marks must be greater than zero.")]
        public decimal? TotalMarks { get; set; }
    }
}
