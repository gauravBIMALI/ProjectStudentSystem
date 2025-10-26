using ProjectStudentSystem.Models;

namespace ProjectStudentSystem.ViewModels
{
    public class UsersListViewModel
    {
        public IEnumerable<Users> Teachers { get; set; } = new List<Users>();
        public IEnumerable<Users> Students { get; set; } = new List<Users>();
    }
}
