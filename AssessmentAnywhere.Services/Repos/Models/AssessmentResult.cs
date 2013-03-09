namespace AssessmentAnywhere.Services.Repos.Models
{
    using System;

    public class AssessmentResult
    {
        public AssessmentResult(string candidateName)
            : this(candidateName, new decimal?())
        {
        }

        public AssessmentResult(string candidateName, decimal? result)
            : this(Guid.NewGuid(), candidateName, result)
        {
        }

        public AssessmentResult(Guid id, string candidateName, decimal? result)
        {
            Id = id;
            CandidateName = candidateName;
            Result = result;
        }

        public Guid Id { get; private set; }

        public string CandidateName { get; private set; }

        public decimal? Result { get; private set; }
    }
}