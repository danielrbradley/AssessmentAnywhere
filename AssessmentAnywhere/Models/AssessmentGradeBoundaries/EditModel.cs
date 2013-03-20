namespace AssessmentAnywhere.Models.AssessmentGradeBoundaries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AssessmentAnywhere.Services.GradeBoundaries;

    public class EditModel
    {
        public Guid AssessmentId { get; private set; }

        public string AssessmentName { get; private set; }

        public decimal? TotalMarks { get; private set; }

        public IList<GradeBoundary> Boundaries { get; private set; }

        public GradeBoundary NewBoundary { get; private set; }

        public EditModel(Guid assessmentId, string assessmentName, decimal? totalMarks, IList<GradeBoundary> boundaries, GradeBoundary newBoundary)
        {
            AssessmentId = assessmentId;
            AssessmentName = assessmentName;
            TotalMarks = totalMarks;
            Boundaries = boundaries;
            NewBoundary = newBoundary;
        }

        public EditModel(Guid assessmentId, string assessmentName, decimal? totalMarks, IList<GradeBoundary> boundaries)
            : this(assessmentId, assessmentName, totalMarks, boundaries, GradeBoundary.New)
        {
        }

        public EditModel(Guid assessmentId, string assessmentName, decimal? totalMarks)
            : this(assessmentId, assessmentName, totalMarks, new List<GradeBoundary>())
        {
        }

        public EditModel(Services.Repos.Models.Assessment assessment)
            : this(assessment.Id, assessment.Name, assessment.TotalMarks)
        {
        }

        public EditModel(Services.Repos.Models.Assessment assessment, IEnumerable<IBoundary> boundaries)
            : this(assessment.Id, assessment.Name, assessment.TotalMarks, ConstructGradeBoundaries(boundaries))
        {
        }

        private static IList<GradeBoundary> ConstructGradeBoundaries(IEnumerable<IBoundary> boundaries)
        {
            return boundaries.OrderBy(b => b.Grade).Select(b => new GradeBoundary(b)).ToList();
        }
    }
}