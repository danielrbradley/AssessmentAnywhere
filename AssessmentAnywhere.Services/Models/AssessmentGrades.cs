namespace AssessmentAnywhere.Services.Models
{
    using System;
    using System.Collections.Generic;

    using AssessmentAnywhere.Services.Repos.Models;

    public class AssessmentGrades
    {
        public AssessmentGrades(Guid assessmentId, string assessmentName, decimal? totalMarks, IEnumerable<CandidateGrade> candidates, IEnumerable<Boundary> boundaries)
        {
            AssessmentId = assessmentId;
            AssessmentName = assessmentName;
            TotalMarks = totalMarks;
            Candidates = candidates;
            Boundaries = boundaries;
        }

        public Guid AssessmentId { get; private set; }

        public string AssessmentName { get; private set; }

        public decimal? TotalMarks { get; private set; }

        public IEnumerable<CandidateGrade> Candidates { get; private set; }

        public IEnumerable<Boundary> Boundaries { get; private set; }
    }
}