﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssessmentAnywhere.Models.Assessments
{
    public class DetailsModel
    {
        public DetailsModel()
        {
            Candidates = new List<Candidate>();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool HasGrades { get; set; }

        public List<Candidate> Candidates { get; set; }

        public List<Assessment> AllAssessments { get; set; }

        public class Candidate
        {
            public Guid Id { get; set; }

            public string Name { get; set; }

            public decimal? Result { get; set; }

            public string Grade { get; set; }
        }
    }
}