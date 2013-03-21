namespace AssessmentAnywhere.Models.AssessmentImport
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using AssessmentAnywhere.Services.Assessments;

    public class UploadViewModel
    {
        public UploadViewModel()
        {
            WorksheetNumber = 1;
            StartRow = 1;
        }

        public UploadViewModel(IAssessment assessment)
            : this()
        {
            this.AssessmentId = assessment.Id;
            this.AssessmentName = assessment.Name;
        }

        public UploadViewModel(IAssessment assessment, UploadPostbackModel model)
            : this(assessment)
        {
            this.WorksheetNumber = model.WorksheetNumber;
            this.SurnameColumn = model.SurnameColumn;
            this.ForenamesColumn = model.ForenamesColumn;
            this.ResultColumn = model.ResultColumn;
            this.StartRow = model.StartRow;
        }

        public Guid AssessmentId { get; private set; }

        public string AssessmentName { get; private set; }

        [Required]
        public int WorksheetNumber { get; private set; }

        [Required]
        public string SurnameColumn { get; private set; }

        [Required]
        public string ForenamesColumn { get; private set; }

        [Required]
        public string ResultColumn { get; private set; }

        [Required]
        public int StartRow { get; private set; }
    }
}