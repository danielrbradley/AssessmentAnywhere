namespace AssessmentAnywhere.Services.Repos
{
    using System;
    using System.Linq;

    using AssessmentAnywhere.Services.Repos.Models;

    public class AssessmentsRepo
    {
        private static readonly CurrentUserDictionary<Guid, Assessment> Assessments = new CurrentUserDictionary<Guid, Assessment>();

        public Assessment Create()
        {
            var newId = Guid.NewGuid();
            var assessment = new Assessment(newId);
            Assessments.Add(newId, assessment);
            return assessment;
        }

        public Assessment Open(Guid assessmentId)
        {
            return Assessments[assessmentId];
        }

        public IQueryable<Assessment> QueryAssessments()
        {
            return Assessments.Values.AsQueryable();
        }

        public void Delete(Guid id)
        {
            Assessments.Remove(id);
        }
    }
}
