namespace AssessmentAnywhere.Models.Assessments
{
    using System;
    using System.Collections.Generic;

    public class UpdateModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public IList<UpdateResultRow> Results { get; set; }
    }
}
