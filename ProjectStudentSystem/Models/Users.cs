using Microsoft.AspNetCore.Identity;

namespace ProjectStudentSystem.Models
{
    public class Users : IdentityUser
    {
        public string FullName { get; set; } = null!;
        public int Age { get; set; }
        public int Role { get; set; }
        public string ProfileImage { get; internal set; }
    }
}
