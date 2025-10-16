using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ProjectStudentSystem.Controllers
{
    public class TeacherController : Controller
    {
        private readonly ILogger<TeacherController> _logger;
        private readonly IWebHostEnvironment _env;

        private readonly RoleManager<IdentityRole> _roleManager;

        public TeacherController(ILogger<TeacherController> logger, IWebHostEnvironment env, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _env = env;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }
    }
}
