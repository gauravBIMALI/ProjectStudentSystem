using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectStudentSystem.Data;
using ProjectStudentSystem.Models;
using ProjectStudentSystem.ViewModels;

namespace ProjectStudentSystem.Controllers
{
    public class AdminController : Controller
    {

        private readonly ILogger<AdminController> _logger;
        private readonly UserManager<Users> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminController(ILogger<AdminController> logger, UserManager<Users> userManager, RoleManager<IdentityRole> roleManager, AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        [Authorize(Roles = "Admin")]

        //public IActionResult Index()
        //{
        //    return View();
        //}


        public async Task<IActionResult> Index()
        {
            var totalUsers = await _context.Users.CountAsync();
            var totalTeachers = await _context.Users.CountAsync(u => u.Role == 1); // 1 = Teacher
            var totalStudents = await _context.Users.CountAsync(u => u.Role == 0); // 0 = Student
            var totalClasses = await _context.AddClasses.CountAsync();
            var totalSubjects = await _context.AddSubjects.CountAsync();
            var model = new AdminDashboardViewModel
            {
                TotalUsers = totalUsers,
                TotalTeachers = totalTeachers,
                TotalStudents = totalStudents,
                TotalClasses = totalClasses,
                TotalSubjects = totalSubjects
            };
            return View(model);
        }


        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var model = new AdminProfileViewModel
            {
                Name = user.FullName ?? "Unknown",
                Email = user.Email ?? "Unknown",
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

            var model = new AdminProfileViewModel
            {
                Name = user.FullName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                ProfileImage = user.ProfileImage
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(AdminProfileViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            // Handle image upload
            if (model.ProfileImageFile != null && model.ProfileImageFile.Length > 0)
            {
                var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif" };
                if (!allowedTypes.Contains(model.ProfileImageFile.ContentType))
                {
                    ModelState.AddModelError("ProfileImageFile", "Only JPG, PNG or GIF images are allowed.");
                    return View(model);
                }

                if (model.ProfileImageFile.Length > 2 * 1024 * 1024)
                {
                    ModelState.AddModelError("ProfileImageFile", "Image must be smaller than 2MB.");
                    return View(model);
                }

                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ProfileImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfileImageFile.CopyToAsync(fileStream);
                }

                // delete old image if exists
                if (!string.IsNullOrEmpty(user.ProfileImage))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, user.ProfileImage.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                        System.IO.File.Delete(oldImagePath);
                }

                user.ProfileImage = "/Images/" + uniqueFileName;
            }

            // Update name/email
            user.FullName = model.Name;
            user.Email = model.Email;
            user.UserName = model.Email;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Profile updated successfully!";
                return RedirectToAction(nameof(Profile));
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }
        public IActionResult CreateClasses() { return View(); }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateClasses([Bind("Section,Class")] AddClassViewModel addclass)
        {
            if (ModelState.IsValid)
            {
                bool exists = await _context.AddClasses
                    .AnyAsync(c => c.Class == addclass.Class && c.Section == addclass.Section);

                if (exists)
                {
                    ModelState.AddModelError(string.Empty, $"Class {addclass.Class}{addclass.Section} already exists.");
                    return View(addclass);
                }

                var entity = new AddClass
                {
                    Section = addclass.Section,
                    Class = addclass.Class
                };

                _context.AddClasses.Add(entity);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Class {addclass.Class}{addclass.Section} created successfully!";
                return RedirectToAction(nameof(ClassesList));
            }

            return View(addclass);
        }



        public async Task<IActionResult> ClassesList()
        {
            return View(await _context.AddClasses.ToListAsync());
        }








        public async Task<IActionResult> UsersList()
        {
            var teachers = await _context.Users
                .Where(u => u.Role == 1) // 1 = Teacher
                .ToListAsync();

            var students = await _context.Users
                .Where(u => u.Role == 0) // 0 = Student
                .ToListAsync();

            var model = new UsersListViewModel
            {
                Teachers = teachers,
                Students = students
            };

            return View(model);
        }
        //subject list
        public async Task<IActionResult> SubjectList()
        {
            var subjects = await _context.AddSubjects
                .OrderBy(s => s.Class)
                .ThenBy(s => s.Subject)
                .ToListAsync();

            return View(subjects);
        }


        public IActionResult AddSubject()
        {
            // Fetch distinct class list (ignore sections)
            ViewBag.ClassList = _context.AddClasses
                .Select(c => c.Class)
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            return View();
        }

        // POST: Add Subject
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSubject(AddSubjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Prevent duplicates (same subject for same class)
                bool exists = await _context.AddSubjects
                    .AnyAsync(s => s.Subject == model.Subject && s.Class == model.Class);

                if (exists)
                {
                    ModelState.AddModelError("", $"Subject '{model.Subject}' already exists for Class {model.Class}.");
                    ViewBag.ClassList = _context.AddClasses
                        .Select(c => c.Class)
                        .Distinct()
                        .OrderBy(c => c)
                        .ToList();
                    return View(model);
                }

                var subject = new AddSubject
                {
                    Subject = model.Subject,
                    Class = model.Class
                };

                _context.AddSubjects.Add(subject);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Subject '{model.Subject}' added successfully for Class {model.Class}.";
                return RedirectToAction(nameof(SubjectList)); // create SubjectList view later
            }

            ViewBag.ClassList = _context.AddClasses
                .Select(c => c.Class)
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            return View(model);
        }
    }
}



