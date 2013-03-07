namespace AssessmentAnywhere.Services.Repos.Models
{
    using System;

    public class AssessmentResult
    {
        public Guid CandidateId { get; set; }

        public decimal? Result { get; set; }
    }
}