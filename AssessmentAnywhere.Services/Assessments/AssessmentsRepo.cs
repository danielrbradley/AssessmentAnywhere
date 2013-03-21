namespace AssessmentAnywhere.Services.Assessments
{
    using System;
    using System.Linq;

    public class AssessmentsRepo : IAssessmentsRepo
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

        public IQueryable<IAssessment> QueryAssessments()
        {
            return Assessments.Values.AsQueryable();
        }

        public void Delete(Guid id)
        {
            Assessments.Remove(id);
        }
    }
}
