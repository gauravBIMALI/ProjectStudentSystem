using System.ComponentModel.DataAnnotations;

namespace ProjectStudentSystem.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Enter valid email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Enter valid password")]
        [DataType(DataType.Password)]

        public string Password { get; set; } = null!;

        public bool RememberMe { get; set; }
    }
}
