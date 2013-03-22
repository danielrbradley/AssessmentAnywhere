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

        public Assessment(Services.AssessmentIndex.IAssessment indexResult)
            : this(indexResult.Id, indexResult.Name)
        {
        }

        public Guid Id { get; private set; }

        public string Name { get; private set; }
    }
}
