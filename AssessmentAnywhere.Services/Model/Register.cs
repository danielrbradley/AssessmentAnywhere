namespace AssessmentAnywhere.Services.Model
{
    using System;
    using System.Collections.Generic;

    public class Register
    {
        public Register()
        {
            Candidates = new List<Candidate>();
        }

        public Guid Id { get; set; }

        public List<Candidate> Candidates { get; set; }
    }
}
