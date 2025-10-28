namespace ProjectStudentSystem.ViewModels
{
    public class TeacherProfileViewModel
    {

        public int Age { get; set; }

        public string? PhoneNumber { get; set; }

        public string Email { get; set; } = null!;

        public string Role { get; set; } = null!;

        public string Name { get; set; } = null!;
        public string? ProfileImage { get; set; }
        public IFormFile? ProfileImageFile { get; set; } // For file upload
    }
}
