using Microsoft.AspNetCore.Identity;
using ProjectStudentSystem.Models;

namespace ProjectStudentSystem.Services.SeedService
{
    public static class SeedAdmin
    {
        public static async Task SeedAdminUserAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<Users>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string adminEmail = "admin@system.com";
            string adminPassword = "Admin@123";
            string adminRole = "Admin";

            // Check if the Admin role exists; if not, create it
            var roleExists = await roleManager.RoleExistsAsync(adminRole);
            if (!roleExists)
            {
                await roleManager.CreateAsync(new IdentityRole(adminRole));
            }

            // Check if the admin user already exists
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var newAdmin = new Users
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "Admin System",
                    EmailConfirmed = true,
                    Role = 2 // Assuming 0=Student, 1=Teacher, 2=Admin
                };

                var createAdmin = await userManager.CreateAsync(newAdmin, adminPassword);
                if (createAdmin.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, adminRole);
                }
            }
        }
    }
}
