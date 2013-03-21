namespace AssessmentAnywhere.Services.Assessments
{
    using System;

    public class AssessmentResult : IAssessmentResult
    {
        public AssessmentResult(string surname, string forenames)
            : this(surname, forenames, new decimal?())
        {
        }

        public AssessmentResult(string surname, string forenames, decimal? result)
            : this(Guid.NewGuid(), surname, forenames, result)
        {
        }

        public AssessmentResult(Guid id, string surname, string forenames, decimal? result)
        {
            this.Id = id;
            this.Surname = surname;
            this.Forenames = forenames;
            this.Result = result;
        }

        public Guid Id { get; private set; }

        public string Surname { get; private set; }

        public string Forenames { get; private set; }

        public decimal? Result { get; private set; }

        public int CompareTo(IAssessmentResult other)
        {
            var surnameSort = string.Compare(this.Surname, other.Surname, StringComparison.OrdinalIgnoreCase);

            if (surnameSort == 0)
            {
                return string.Compare(this.Forenames, other.Forenames, StringComparison.OrdinalIgnoreCase);
            }

            return surnameSort;
        }
    }
}