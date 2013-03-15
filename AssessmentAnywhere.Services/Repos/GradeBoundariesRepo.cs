namespace AssessmentAnywhere.Services.Repos
{
    using System;
    using System.Collections.Generic;

    using AssessmentAnywhere.Services.Repos.Models;

    public class GradeBoundariesRepo
    {
        private static readonly CurrentUserDictionary<Guid, GradeBoundaries> GradeBoundaries = new CurrentUserDictionary<Guid, GradeBoundaries>();

        public GradeBoundaries Create(Guid assessmentId)
        {
            var boundaries = new GradeBoundaries(assessmentId);
            GradeBoundaries.Add(assessmentId, boundaries);
            return boundaries;
        }

        public GradeBoundaries TryOpen(Guid assessmentId, out bool success)
        {
            if (GradeBoundaries.ContainsKey(assessmentId))
            {
                success = true;
                return GradeBoundaries[assessmentId];
            }

            success = false;
            return null;
        }
    }
}
