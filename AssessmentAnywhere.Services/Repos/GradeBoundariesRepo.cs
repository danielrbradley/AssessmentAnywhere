namespace AssessmentAnywhere.Services.Repos
{
    using System;
    using System.Collections.Generic;

    using AssessmentAnywhere.Services.Repos.Models;

    public class GradeBoundariesRepo
    {
        private static readonly Dictionary<Guid, GradeBoundaries> GradeBoundaries = new Dictionary<Guid, GradeBoundaries>();

        public GradeBoundaries Create(Guid assessmentId)
        {
            var gradeBoundaries = new GradeBoundaries { AssessmentId = assessmentId };
            GradeBoundaries.Add(assessmentId, gradeBoundaries);
            return gradeBoundaries;
        }

        public GradeBoundaries Open(Guid assessmentId)
        {
            if (!GradeBoundaries.ContainsKey(assessmentId))
            {
                GradeBoundaries.Add(assessmentId, new GradeBoundaries { AssessmentId = assessmentId });
            }

            return GradeBoundaries[assessmentId];
        }
    }
}
