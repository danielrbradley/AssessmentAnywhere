namespace AssessmentAnywhere.Models.Assessments
{
    using System;

    public class DetailsModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DetailsModel(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public DetailsModel(Services.Repos.Models.Assessment assessment)
            : this(assessment.Id, assessment.Name)
        {
        }
    }
}
