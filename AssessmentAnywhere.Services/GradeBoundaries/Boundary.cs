namespace AssessmentAnywhere.Services.GradeBoundaries
{
    public class Boundary : IBoundary
    {
        public decimal MinResult { get; private set; }

        public string Grade { get; private set; }

        public Boundary(string grade, decimal minResult)
        {
            this.Grade = grade;
            this.MinResult = minResult;
        }
    }
}
