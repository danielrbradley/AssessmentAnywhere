namespace AssessmentAnywhere.Services.Repos.Models
{
    using System;
    using System.Collections.Generic;

    public class Assessment
    {
        public Assessment()
        {
            this.Results = new List<AssessmentResult>();
        }

        public Guid Id { get; set; }

        public List<AssessmentResult> Results { get; set; }

        public string Name { get; set; }
    }
}