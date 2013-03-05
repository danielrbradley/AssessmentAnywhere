using System.Collections.Generic;

namespace AssessmentAnywhere.Models.Assessments
{
    public class IndexModel
    {
        public IndexModel()
        {
            Assessments = new List<Assessment>();
        }

        public List<Assessment> Assessments { get; set; }
    }
}
