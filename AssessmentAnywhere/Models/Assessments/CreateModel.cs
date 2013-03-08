namespace AssessmentAnywhere.Models.Assessments
{
    using System.ComponentModel.DataAnnotations;

    public class CreateModel
    {
        public CreateModel()
            : this(string.Empty)
        {
        }

        public CreateModel(string name)
        {
            Name = name;
        }

        [Required(ErrorMessage = "The assessment name is required")]
        public string Name { get; set; }
    }
}
