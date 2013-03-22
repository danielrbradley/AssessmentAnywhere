namespace AssessmentAnywhere.Services.AssessmentIndex
{
    using System;

    public interface IAssessment : IComparable<IAssessment>
    {
        Guid Id { get; }

        string Name { get; }
    }
}