namespace AssessmentAnywhere.Services.Repos.Models
{
    using System;

    public class AssessmentResult
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
            Id = id;
            Surname = surname;
            Forenames = forenames;
            Result = result;
        }

        public Guid Id { get; private set; }

        public string Surname { get; private set; }

        public string Forenames { get; private set; }

        public decimal? Result { get; private set; }
    }
}