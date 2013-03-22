namespace AssessmentAnywhere.Services.Assessments
{
    using System;

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

        public void Delete(Guid id)
        {
            Assessments.Remove(id);
        }
    }
}
