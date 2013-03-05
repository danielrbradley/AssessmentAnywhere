namespace AssessmentAnywhere.Services.Model
{
    using System;
    using System.Collections.Generic;

    public class GradeBoundaries
    {
        public Guid AssessmentId { get; set; }

        public List<Boundary> Boundaries { get; set; }
    }
}