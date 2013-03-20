namespace AssessmentAnywhere.Services.GradeBoundaries
{
    using System;
    using System.Collections.Generic;

    public interface IGradeBoundaries
    {
        Guid AssessmentId { get; }

        IList<IBoundary> Boundaries { get; }

        void SetBoundaries(IEnumerable<IBoundary> newBoundaries);

        void AddBoundary(string grade, decimal minResult);

        void UpdateBoundary(string grade, decimal minResult);

        void RemoveBoundary(string grade);
    }
}