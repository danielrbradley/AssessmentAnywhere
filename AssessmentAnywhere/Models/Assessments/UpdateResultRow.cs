﻿namespace AssessmentAnywhere.Models.Assessments
{
    using System;

    public class UpdateResultRow
    {
        public Guid RowId { get; set; }

        public string Surname { get; set; }

        public string Forenames { get; set; }

        public decimal? Result { get; set; }
    }
}