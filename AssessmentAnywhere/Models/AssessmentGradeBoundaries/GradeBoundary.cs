namespace AssessmentAnywhere.Models.AssessmentGradeBoundaries
{
    public class GradeBoundary
    {
        public string Grade { get; private set; }

        public decimal? MinResult { get; private set; }

        public static GradeBoundary New
        {
            get
            {
                return new GradeBoundary(string.Empty, null);
            }
        }

        public GradeBoundary(string grade, decimal? minResult)
        {
            Grade = grade;
            MinResult = minResult;
        }

        public GradeBoundary(Services.Repos.Models.Boundary boundary)
            : this(boundary.Grade, boundary.MinResult)
        {
        }
    }
}