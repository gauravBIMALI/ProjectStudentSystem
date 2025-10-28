using System.ComponentModel.DataAnnotations;

namespace ProjectStudentSystem.ViewModels
{
    public class AddSubjectViewModel
    {
        [Required(ErrorMessage = "Please enter the subject name.")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Please select a class.")]
        public int Class { get; set; }
    }
}
