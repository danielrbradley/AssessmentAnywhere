namespace AssessmentAnywhere.Services.Assessments
{
    using System;

    public interface IAssessmentsRepo
    {
        IAssessment Create();

        IAssessment Open(Guid assessmentId);

        void Delete(Guid id);
    }
}
