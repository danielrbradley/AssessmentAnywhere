namespace AssessmentAnywhere.Services.Services
{
    using System;
    using System.Collections.Generic;

    using AssessmentAnywhere.Services.Model;

    public class AssessmentGrades
    {
        public Guid AsssessmentId { get; set; }

        public string AssessmentName { get; set; }

        public List<CandidateGrade> Candidates { get; set; }

        public List<Boundary> Boundaries { get; set; }
    }
}