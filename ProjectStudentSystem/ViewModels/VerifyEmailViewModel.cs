using System.ComponentModel.DataAnnotations;

namespace ProjectStudentSystem.ViewModels
{
    public class VerifyEmailViewModel
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Enter valid email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;
    }
}
