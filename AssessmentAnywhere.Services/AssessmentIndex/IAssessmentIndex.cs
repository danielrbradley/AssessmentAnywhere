namespace AssessmentAnywhere.Services.AssessmentIndex
{
    using System;

    public interface IAssessmentIndex
    {
        IResultPage Search(int skip, int top);

        bool ContainsName(string name);

        void Set(Guid assessmentId, string name);

        void Delete(Guid assessmentId);
    }
}
