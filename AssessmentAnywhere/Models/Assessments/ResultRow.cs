namespace AssessmentAnywhere.Models.Assessments
{
    using System;

    public class ResultRow
    {
        public ResultRow(Guid rowId, string candidateName, decimal? result)
            : this(rowId, candidateName, result, null, string.Empty)
        {
        }

        public ResultRow(Guid rowId, string candidateName, decimal? result, decimal? percentage, string grade)
        {
            this.RowId = rowId;
            this.CandidateName = candidateName;
            this.Result = result;
            this.Percentage = percentage;
            this.Grade = grade;
        }

        public Guid RowId { get; private set; }

        public string CandidateName { get; private set; }

        public decimal? Result { get; private set; }

        public decimal? Percentage { get; private set; }

        public string Grade { get; private set; }
    }
}
