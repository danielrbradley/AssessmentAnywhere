namespace AssessmentAnywhere.Services.GradeBoundaries
{
    using System;

    public interface IGradeBoundariesRepo
    {
        IGradeBoundaries Create(Guid assessmentId);

        IGradeBoundaries TryOpen(Guid assessmentId, out bool success);
    }
}
