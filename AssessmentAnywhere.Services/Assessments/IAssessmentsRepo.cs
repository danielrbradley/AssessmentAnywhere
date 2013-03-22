namespace AssessmentAnywhere.Services.Assessments
{
    using System;

    public interface IAssessmentsRepo
    {
        Assessment Create();

        Assessment Open(Guid assessmentId);

        void Delete(Guid id);
    }
}
