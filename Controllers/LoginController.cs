using GymTrainingSystem.Data;
using GymTrainingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static GymTrainingSystem.Models.MetaClass;
using static GymTrainingSystem.Helper.ApplicationHelper;

namespace GymTrainingSystem.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext dbContext;

        public LoginController(AppDbContext context)
        {
            dbContext = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Models.User user,string RememberMe)
        {
            // Verify user credentials
            var verify = dbContext.Users.FirstOrDefault(u =>
                u.Email == user.Email &&
                u.Password == user.Password &&
                !u.IsDeleted);

            if (verify == null)
            {
                TempData["Error"] = "Invalid email or password";
                return RedirectToAction("Index", "Login");
            }
            SetUserSession(HttpContext, verify);


            return RedirectToAction("Dashboard", "Home");
        }
        [HttpGet]
        public IActionResult Logout()
        {
            // Remove the saved UserSession
            HttpContext.Session.Remove("UserSession");

            // Or clear everything (if you want to wipe all sessions)
            // HttpContext.Session.Clear();

            // Redirect to login page
            return RedirectToAction("Index", "Login");
        }



    }
}
