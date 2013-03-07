namespace AssessmentAnywhere.Services.Repos.Models
{
    using System;
    using System.Collections.Generic;

    public class GradeBoundaries
    {
        public GradeBoundaries()
        {
            this.Boundaries = new List<Boundary>();
        }

        public Guid AssessmentId { get; set; }

        public List<Boundary> Boundaries { get; set; }
    }
}