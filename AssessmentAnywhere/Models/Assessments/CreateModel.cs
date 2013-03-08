namespace AssessmentAnywhere.Models.Assessments
{
    using System.Collections.Generic;
    using System.Linq;

    public class CreateModel
    {
        public CreateModel()
            : this(string.Empty, string.Empty, Enumerable.Empty<string>())
        {
        }

        public CreateModel(string name, string selectedSubject, IEnumerable<string> availableSubjects)
        {
            Name = name;
            SelectedSubject = selectedSubject;
            AvailableSubjects = availableSubjects;
        }

        public string Name { get; set; }

        public string SelectedSubject { get; set; }

        public IEnumerable<string> AvailableSubjects { get; set; }
    }
}
