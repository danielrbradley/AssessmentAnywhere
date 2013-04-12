namespace AssessmentAnywhere.Services.Assessments
{
    using System;

    internal class AssessmentsRepo : IAssessmentsRepo
    {
        private static readonly CurrentUserDictionary<Guid, Assessment> Assessments = new CurrentUserDictionary<Guid, Assessment>();

        public IAssessment Create()
        {
            var newId = Guid.NewGuid();
            var assessment = new Assessment(newId);
            Assessments.Add(newId, assessment);
            return assessment;
        }

        public IAssessment Open(Guid assessmentId)
        {
            return Assessments[assessmentId];
        }

        public void Delete(Guid id)
        {
            Assessments.Remove(id);
        }
    }
}
