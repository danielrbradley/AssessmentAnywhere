namespace AssessmentAnywhere.Services.GradeBoundaries
{
    using System;

    public class GradeBoundariesRepo : IGradeBoundariesRepo
    {
        private static readonly CurrentUserDictionary<Guid, GradeBoundaries> GradeBoundaries = new CurrentUserDictionary<Guid, GradeBoundaries>();

        public IGradeBoundaries Create(Guid assessmentId)
        {
            var boundaries = new GradeBoundaries(assessmentId);
            GradeBoundaries.Add(assessmentId, boundaries);
            return boundaries;
        }

        public IGradeBoundaries TryOpen(Guid assessmentId, out bool success)
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
