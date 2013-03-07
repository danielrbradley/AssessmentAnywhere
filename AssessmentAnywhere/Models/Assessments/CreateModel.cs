namespace AssessmentAnywhere.Models.Assessments
{
    using System;
    using System.Collections.Generic;

    public class CreateModel
    {
        public string Name { get; set; }

        public string SelectedSubject { get; set; }

        public List<string> AvailableSubjects { get; set; }

        public class Register
        {
            public Guid Id { get; set; }

            public string Name { get; set; }
        }
    }
}