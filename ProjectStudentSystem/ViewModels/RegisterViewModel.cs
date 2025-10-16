using System.ComponentModel.DataAnnotations;

namespace ProjectStudentSystem.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Enter valid age")]
        [Display(Name = "Age")]
        [Range(4, 55, ErrorMessage = "Age must be between 4 and 55")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Enter valid phone")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        [Phone]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits")]
        public string PhoneNumber { get; set; } = null!;

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Enter valid email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Select your role")]
        [Display(Name = "Role")]
        public string Role { get; set; }

        [Required(ErrorMessage = "Enter valid password")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string Password { get; set; } = null!;
        [Required(ErrorMessage = "Confirm your password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = null!;
        [Required(ErrorMessage = "Enter your name")]
        [Display(Name = "Full Name")]

        public string Name { get; set; } = null!;
    }
}
