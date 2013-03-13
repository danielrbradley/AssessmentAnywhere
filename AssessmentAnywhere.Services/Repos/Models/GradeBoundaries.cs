namespace AssessmentAnywhere.Services.Repos.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class GradeBoundaries
    {
        private Dictionary<string, Boundary> boundaries = new Dictionary<string, Boundary>();

        public GradeBoundaries(Guid assessmentId)
        {
            AssessmentId = assessmentId;
        }

        public Guid AssessmentId { get; private set; }

        public IList<Boundary> Boundaries
        {
            get
            {
                return boundaries.Values.OrderBy(b => b.Grade).ToList();
            }
        }

        public void SetBoundaries(IEnumerable<Boundary> newBoundaries)
        {
            var newBoundariesList = newBoundaries as List<Boundary> ?? newBoundaries.ToList();
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