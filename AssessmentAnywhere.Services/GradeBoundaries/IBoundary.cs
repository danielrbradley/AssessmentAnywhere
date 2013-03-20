namespace AssessmentAnywhere.Services.GradeBoundaries
{
    public interface IBoundary
    {
        decimal MinResult { get; }

        string Grade { get; }
    }
}