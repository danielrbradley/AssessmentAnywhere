namespace AssessmentAnywhere.Services.AssessmentIndex
{
    using System;

    internal class Assessment : IAssessment
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public Assessment(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public int CompareTo(IAssessment other)
        {
            return string.CompareOrdinal(this.Name, other.Name);
        }
    }
}
