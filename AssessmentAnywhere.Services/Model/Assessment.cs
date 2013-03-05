namespace AssessmentAnywhere.Services.Model
{
    using System;
    using System.Collections.Generic;

    public class Assessment
    {
        public Assessment()
        {
            Results = new List<AssessmentResult>();
        }

        public Guid Id { get; set; }

        public Guid RegisterId { get; set; }

        public List<AssessmentResult> Results { get; set; }
    }
}