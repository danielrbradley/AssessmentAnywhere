namespace AssessmentAnywhere.Services.Repos.Models
{
    public class Boundary
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