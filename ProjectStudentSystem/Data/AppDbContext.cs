using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectStudentSystem.Models;

namespace ProjectStudentSystem.Data
{
    public class AppDbContext : IdentityDbContext<Users>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AddClass> AddClasses { get; set; } = default!;
        public DbSet<Teacher> Teachers { get; set; } = default!;
    }
}
