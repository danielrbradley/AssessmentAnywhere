namespace AssessmentAnywhere.Services.Assessments
{
    using System;
    using System.Collections.Generic;

    public interface IAssessment
    {
        Guid Id { get; }

        string Name { get; }

        decimal? TotalMarks { get; }

        IList<IAssessmentResult> Results { get; }

        Guid AddCandidate(string surname, string forenames);

        void SetName(string name);

        void SetTotalMarks(decimal? totalMarks);

        void SetCandidateNames(Guid id, string surname, string forenames);

        void SetCandidateResult(Guid id, decimal? result);

        void RemoveResult(Guid id);
    }
}