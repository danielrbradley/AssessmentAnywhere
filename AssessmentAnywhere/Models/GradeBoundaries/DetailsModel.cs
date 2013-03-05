using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssessmentAnywhere.Models.GradeBoundaries
{
    public class DetailsModel
    {
        public Guid AssessmentId { get; set; }

        public string AssessmentName { get; set; }

        public List<Boundary> Boundaries { get; set; }

        public class Boundary
        {
            public int MinResult { get; set; }

            public string Grade { get; set; }
        }
    }
}
