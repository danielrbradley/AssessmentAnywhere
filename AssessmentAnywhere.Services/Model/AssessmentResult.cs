namespace AssessmentAnywhere.Services.Model
{
    using System;

    public class AssessmentResult
    {
        public Guid CandidateId { get; set; }

        public decimal? Result { get; set; }
    }
}