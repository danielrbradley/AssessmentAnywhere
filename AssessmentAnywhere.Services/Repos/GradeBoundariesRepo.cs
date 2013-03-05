namespace AssessmentAnywhere.Services.Repos
{
    using System;
    using System.Collections.Generic;

    using AssessmentAnywhere.Services.Model;

    public class GradeBoundariesRepo
    {
        private static readonly Dictionary<Guid, GradeBoundaries> GradeBoundaries = new Dictionary<Guid, GradeBoundaries>();

        public GradeBoundaries Create(Guid assessmentId)
        {
            var newId = Guid.NewGuid();
            var gradeBoundaries = new GradeBoundaries { AssessmentId = assessmentId };
            GradeBoundaries.Add(newId, gradeBoundaries);
            return gradeBoundaries;
        }

        public GradeBoundaries Open(Guid assessmentId)
        {
            return GradeBoundaries[assessmentId];
        }
    }
}
