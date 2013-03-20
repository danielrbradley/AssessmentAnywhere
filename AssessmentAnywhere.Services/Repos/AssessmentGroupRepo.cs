namespace AssessmentAnywhere.Services.Repos
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AssessmentAnywhere.Services.GradeBoundaries;
    using AssessmentAnywhere.Services.Repos.Models;

    public class AssessmentGroupRepo
    {
        private static readonly CurrentUserDictionary<Guid, AssessmentGroup> AssessmentGroups = new CurrentUserDictionary<Guid, AssessmentGroup>();

        public AssessmentGroup Create(List<Guid> assessmentIds, GradeBoundaries boundaries)
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

        public List<AssessmentGroup> GetAssessmentGroups()
        {
            return AssessmentGroups.Values.ToList();
        }
    }
}
