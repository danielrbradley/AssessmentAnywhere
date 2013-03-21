namespace AssessmentAnywhere.Services.Assessments
{
    using System;

    public interface IAssessmentResult : IComparable<IAssessmentResult>
    {
        Guid Id { get; }

        string Surname { get; }

        string Forenames { get; }

        decimal? Result { get; }
    }
}