namespace ProjectStudentSystem.ViewModels
{
    public class AdminProfileViewModel
    {
        public string Email { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string? ProfileImage { get; set; }
        public IFormFile? ProfileImageFile { get; set; } // For file upload
    }
}
