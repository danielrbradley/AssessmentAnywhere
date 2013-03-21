namespace AssessmentAnywhere.Models.Assessments
{
    using System;

    public class DeleteModel
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public DeleteModel(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public DeleteModel(Services.Assessments.IAssessment assessment)
            : this(assessment.Id, assessment.Name)
        {
        }
    }
}