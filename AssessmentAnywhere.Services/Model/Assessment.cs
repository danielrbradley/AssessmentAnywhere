namespace AssessmentAnywhere.Services.Model
{
    using System;
    using System.Collections.Generic;

    public class Assessment
    {
        public Guid Id { get; set; }

        public Guid RegisterId { get; set; }

        public List<AssessmentResult> Results { get; set; }
    }
}