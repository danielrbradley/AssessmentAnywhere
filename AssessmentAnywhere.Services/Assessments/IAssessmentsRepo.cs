namespace AssessmentAnywhere.Services.Assessments
{
    using System;
    using System.Linq;

    public interface IAssessmentsRepo
    {
        Assessment Create();

        Assessment Open(Guid assessmentId);

        IQueryable<IAssessment> QueryAssessments();

        void Delete(Guid id);
    }
}