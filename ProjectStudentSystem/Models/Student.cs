using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectStudentSystem.Models
{
    public class Student
    {
        public int Id { get; set; }
        public int Age { get; set; }

        public int PhoneNumber { get; set; }

        public string Email { get; set; } = null!;

        public string Role { get; set; } = null!;

        public string Name { get; set; } = null!;
        public string? ProfileImage { get; set; }
        [NotMapped]
        public IFormFile? ProfileImageFile { get; set; } // For file upload
    }
}
