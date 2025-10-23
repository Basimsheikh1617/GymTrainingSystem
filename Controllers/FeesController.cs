using Microsoft.AspNetCore.Mvc;

namespace GymTrainingSystem.Controllers
{
    public class FeesController : Controller
    {
        public IActionResult PaidFees()
        {
            return View();
        }
    }
}
