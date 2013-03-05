using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssessmentAnywhere.Services.Model;

namespace AssessmentAnywhere.Services.Repos
{
    public class AssessmentGroupRepo
    {
        private static readonly Dictionary<Guid, AssessmentGroup> AssessmentGroups = new Dictionary<Guid, AssessmentGroup>();

        public AssessmentGroup Create(List<Guid> assessmentIds, GradeBoundaries boundaries )
        {
            var newId = Guid.NewGuid();
            var assessmentGroup = new AssessmentGroup { AssessmentGroupId = newId, AssessmentIds = assessmentIds, Boundaries = boundaries};
            AssessmentGroups.Add(newId, assessmentGroup);
            return assessmentGroup;
        }

        public AssessmentGroup Open(Guid assessmentId)
        {
            return AssessmentGroups[assessmentId];
        }
    }
}
