namespace AssessmentAnywhere.Services.Repos.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class GradeBoundaries
    {
        private readonly Dictionary<string, Boundary> boundaries = new Dictionary<string, Boundary>();

        public GradeBoundaries(Guid assessmentId, decimal maxResult)
        {
            AssessmentId = assessmentId;
            MaxResult = maxResult;
        }

        public Guid AssessmentId { get; private set; }

        public decimal MaxResult { get; private set; }

        public IEnumerable<Boundary> Boundaries
        {
            get
            {
                return boundaries.Values;
            }
        }

        public void SetMaxResult(decimal maxResult)
        {
            this.MaxResult = maxResult;
        }

        public void AddBoundary(string grade, decimal minResult)
        {
            boundaries.Add(grade, new Boundary(grade, minResult));
        }

        public void UpdateBoundary(string grade, decimal minResult)
        {
            boundaries[grade] = new Boundary(grade, minResult);
        }

        public void RemoveBoundary(string grade)
        {
            boundaries.Remove(grade);
        }
    }
}