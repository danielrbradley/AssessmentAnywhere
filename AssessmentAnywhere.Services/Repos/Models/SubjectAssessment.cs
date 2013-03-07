namespace AssessmentAnywhere.Services.Repos.Models
{
    using System;
    using System.Collections.Generic;

    public class SubjectAssessment
    {
        public string Subject{ get; set; }

        public List<Guid> AssessmentIds { get; set; }
    }
}
