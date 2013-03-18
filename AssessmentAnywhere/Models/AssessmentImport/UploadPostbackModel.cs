namespace AssessmentAnywhere.Models.AssessmentImport
{
    using System.ComponentModel.DataAnnotations;

    public class UploadPostbackModel
    {
        public int WorksheetNumber { get; set; }

        [Required]
        public string SurnameColumn { get; set; }

        [Required]
        public string ForenamesColumn { get; set; }

        [Required]
        public string ResultColumn { get; set; }

        [Required]
        public int StartRow { get; set; }
    }
}