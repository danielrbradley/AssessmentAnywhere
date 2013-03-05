using System;
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

        public class Assessment
        {
            public Guid Id { get; set; }

            public string Name { get; set; }
        }
    }
}