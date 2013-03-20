namespace AssessmentAnywhere.Services.GradeBoundaries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class GradeBoundaries : IGradeBoundaries
    {
        private Dictionary<string, IBoundary> boundaries = new Dictionary<string, IBoundary>();

        public GradeBoundaries(Guid assessmentId)
        {
            this.AssessmentId = assessmentId;
        }

        public Guid AssessmentId { get; private set; }

        public IList<IBoundary> Boundaries
        {
            get
            {
                return this.boundaries.Values.OrderBy(b => b.Grade).ToList();
            }
        }

        public void SetBoundaries(IEnumerable<IBoundary> newBoundaries)
        {
            var newBoundariesList = newBoundaries as List<IBoundary> ?? newBoundaries.ToList();
            if (newBoundariesList.Select(b => b.Grade).Distinct().Count() != newBoundariesList.Count)
            {
                throw new ArgumentException("Boundary grades must be unique.", "newBoundaries");
            }

            if (newBoundariesList.Select(b => b.MinResult).Distinct().Count() != newBoundariesList.Count)
            {
                throw new ArgumentException("Boundary min results must be unique.", "newBoundaries");
            }

            this.boundaries = newBoundariesList.ToDictionary(b => b.Grade);
        }

        public void AddBoundary(string grade, decimal minResult)
        {
            this.boundaries.Add(grade, new Boundary(grade, minResult));
        }

        public void UpdateBoundary(string grade, decimal minResult)
        {
            this.boundaries[grade] = new Boundary(grade, minResult);
        }

        public void RemoveBoundary(string grade)
        {
            this.boundaries.Remove(grade);
        }
    }
}