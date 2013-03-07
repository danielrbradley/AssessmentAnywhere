namespace AssessmentAnywhere.Services.Models
{
    using System;

    public class CandidateGrade
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public decimal? Result { get; set; }

        public string Grade { get; set; }
    }
}
