namespace AssessmentAnywhere.Models.Assessments
{
    using System;

    public class UpdateResultRow
    {
        public Guid RowId { get; set; }

        public string CandidateName { get; set; }

        public decimal? Result { get; set; }
    }
}
