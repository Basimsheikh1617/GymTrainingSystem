using GymTrainingSystem.Data;
using GymTrainingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static GymTrainingSystem.Models.MetaClass;

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
                // Invalid login, redirect to login page
                TempData["Error"] = "Invalid email or password";
                return RedirectToAction("Index", "Login");
            }
      

            // Successful login, redirect to dashboard or home
            return RedirectToAction("Index", "Home");
        }

    }
}
