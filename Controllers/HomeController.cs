using GymTrainingSystem.Data;
using GymTrainingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using static GymTrainingSystem.Helper.ApplicationHelper;

namespace GymTrainingSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext dbContext;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            dbContext = context;
        }

        public IActionResult AddMember()
        {
            var sessionUser = GetUserSession(HttpContext);

            if (sessionUser == null)
            {
                return RedirectToAction("Index", "Login"); // agar session khatam ho gaya
            }
                           return View();
        }

        public IActionResult Dashboard()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddMember([FromForm] Member member)
        {
           
                dbContext.Members.Add(member);
                dbContext.SaveChanges();
                return Json(new { success = true });
            
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
