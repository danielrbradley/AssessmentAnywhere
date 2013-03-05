using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssessmentAnywhere.Services.Model
{
    public class AssessmentGroup
    {
        public AssessmentGroup()
        {
            AssessmentIds = new List<Guid>();
        }

        public Guid AssessmentGroupId { get; set; }
        public GradeBoundaries  Boundaries { get; set; }
        public List<Guid> AssessmentIds { get; set; }
    }
}
