namespace AssessmentAnywhere.Models.Assessments
{
    using System.Collections.Generic;

    public class IndexModel
    {
        public IndexModel(IEnumerable<Assessment> assessments)
        {
            Assessments = assessments;
        }

        public IEnumerable<Assessment> Assessments { get; private set; }
    }
}
