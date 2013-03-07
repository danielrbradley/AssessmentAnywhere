namespace AssessmentAnywhere.Services.Repos.Models
{
    using System;
    using System.Collections.Generic;

    public class Register
    {
        public Register()
        {
            this.Candidates = new List<Candidate>();
        }

        public Guid Id { get; set; }

        public List<Candidate> Candidates { get; set; }

        public string Name { get; set; }
    }
}
