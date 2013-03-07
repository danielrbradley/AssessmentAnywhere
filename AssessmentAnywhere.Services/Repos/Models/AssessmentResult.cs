namespace AssessmentAnywhere.Services.Repos.Models
{
    public class AssessmentResult
    {
        public AssessmentResult(string candidateName)
            : this(candidateName, new decimal?())
        {
        }

        public AssessmentResult(string candidateName, decimal? result)
        {
            CandidateName = candidateName;
            Result = result;
        }

        public string CandidateName { get; private set; }

        public decimal? Result { get; private set; }
    }
}