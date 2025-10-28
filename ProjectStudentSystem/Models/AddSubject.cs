using System.ComponentModel.DataAnnotations;

namespace ProjectStudentSystem.Models
{
    public class AddSubject
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Subject name is required.")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Class is required.")]
        public int Class { get; set; } // Linked to AddClassViewModel.Class
    }
}
