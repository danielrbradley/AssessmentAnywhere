namespace AssessmentAnywhere.Models.Assessments
{
    using System;

    public class Assessment
    {
        public Assessment(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Assessment(Services.Assessments.IAssessment assessment)
            : this(assessment.Id, assessment.Name)
        {
        }

        public Guid Id { get; private set; }

        public string Name { get; private set; }
    }
}
