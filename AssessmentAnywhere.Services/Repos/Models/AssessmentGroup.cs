namespace AssessmentAnywhere.Services.Repos.Models
{
    using System;
    using System.Collections.Generic;

    using AssessmentAnywhere.Services.GradeBoundaries;

    public class AssessmentGroup
    {
        public AssessmentGroup()
        {
            this.AssessmentIds = new List<Guid>();
        }

        public Guid AssessmentGroupId { get; set; }

        public GradeBoundaries  Boundaries { get; set; }

        public List<Guid> AssessmentIds { get; set; }
    }
}
