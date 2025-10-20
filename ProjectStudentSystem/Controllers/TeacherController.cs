using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectStudentSystem.Data;
using ProjectStudentSystem.Models;
using ProjectStudentSystem.ViewModels;

namespace ProjectStudentSystem.Controllers
{
    public class TeacherController : Controller
    {
        private readonly ILogger<TeacherController> _logger;
        private readonly UserManager<Users> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TeacherController(ILogger<TeacherController> logger, UserManager<Users> userManager, RoleManager<IdentityRole> roleManager, AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            var model = new ViewModels.TeacherProfileViewModel
            {
                Name = user.FullName ?? "Unknown",
                Email = user.Email ?? "Unknown",
                PhoneNumber = user.PhoneNumber != null ? int.Parse(user.PhoneNumber) : 0,
                Age = user.Age,
                Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "Unknown",
                ProfileImage = user.ProfileImage // file path
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var model = new TeacherProfileViewModel
            {
                Name = user.FullName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                ProfileImage = user.ProfileImage,
                Age = user.Age,
                PhoneNumber = user.PhoneNumber != null ? int.Parse(user.PhoneNumber) : 0,
                Role = user.Role.ToString(),
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(ViewModels.TeacherProfileViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                user.FullName = model.Name;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber.ToString();
                user.Age = model.Age;
                // Handle profile image upload
                if (model.ProfileImageFile != null && model.ProfileImageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    Directory.CreateDirectory(uploadsFolder); // Ensure the directory exists
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ProfileImageFile.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ProfileImageFile.CopyToAsync(fileStream);
                    }
                    user.ProfileImage = "/uploads/" + uniqueFileName; // Save relative path
                }
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Profile");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }
    }
}
