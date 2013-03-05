using System.Linq;
using AssessmentAnywhere.Services.Model;

namespace AssessmentAnywhere.Services.Repos
{
    using System;
    using System.Collections.Generic;


    public class AssessmentsRepo
    {
        private static readonly Dictionary<Guid, Assessment> Assessments = new Dictionary<Guid, Assessment>();

        public Assessment Create(Guid registerId)
        {
            var newId = Guid.NewGuid();
            var assessment = new Assessment { Id = newId, RegisterId = registerId };
            Assessments.Add(newId, assessment);
            return assessment;
        }

        public Assessment Open(Guid assessmentId)
        {
            return Assessments[assessmentId];
        }

        public List<Assessment> GetAssessments()
        {
            return Assessments.Values.ToList();
        }
    }
}
