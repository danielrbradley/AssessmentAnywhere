namespace AssessmentAnywhere.Models.Assessments
{
    using System;
    using System.Collections.Generic;

    public class CreateModel
    {
        public CreateModel()
        {
            ExistingRegisters = new List<Register>();
        }

        public string Name { get; set; }

        public Guid? SelectedRegisterId { get; set; }

        public List<Register> ExistingRegisters { get; set; }

        public class Register
        {
            public Guid Id { get; set; }

            public string Name { get; set; }
        }
    }
}