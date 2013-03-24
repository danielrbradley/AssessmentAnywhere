namespace AssessmentAnywhere.Models.Api.Assessments
{
    using System;
    using System.Runtime.Serialization;

    public class Result
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}