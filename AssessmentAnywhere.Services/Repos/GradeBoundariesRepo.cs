namespace AssessmentAnywhere.Services.Repos
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    using AssessmentAnywhere.Services.Repos.Models;

    public class GradeBoundariesRepo
    {
        private static readonly ConcurrentDictionary<Guid, GradeBoundaries> GradeBoundaries = new ConcurrentDictionary<Guid, GradeBoundaries>();

        public GradeBoundaries OpenOrCreate(Guid assessmentId)
        {
            if (!GradeBoundaries.ContainsKey(assessmentId))
            {
                GradeBoundaries.TryAdd(assessmentId, new GradeBoundaries { AssessmentId = assessmentId });
            }

            return GradeBoundaries[assessmentId];
        }
    }
}
