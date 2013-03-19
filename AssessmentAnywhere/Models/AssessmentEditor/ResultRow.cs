namespace AssessmentAnywhere.Models.AssessmentEditor
{
    using System;

    public class ResultRow
    {
        public ResultRow(Guid rowId, string surname, string forenames, decimal? result, decimal? percentage)
            : this(rowId, surname, forenames, result, percentage, string.Empty)
        {
        }

        public ResultRow(Guid rowId, string surname, string forenames, decimal? result, decimal? percentage, string grade)
        {
            this.RowId = rowId;
            this.Surname = surname;
            this.Forenames = forenames;
            this.Result = result;
            this.Percentage = percentage;
            this.Grade = grade;
        }

        public Guid RowId { get; private set; }

        public string Surname { get; private set; }

        public string Forenames { get; private set; }

        public decimal? Result { get; private set; }

        public decimal? Percentage { get; private set; }

        public string Grade { get; private set; }

        public static readonly ResultRow NewRow = new ResultRow(Guid.Empty, string.Empty, string.Empty, null, null);
    }
}
