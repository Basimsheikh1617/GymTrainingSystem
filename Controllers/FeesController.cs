using GymTrainingSystem.Data;
using GymTrainingSystem.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

using static GymTrainingSystem.Helper.ApplicationHelper;

namespace GymTrainingSystem.Controllers
{
    public class FeesController : Controller
    {
        private readonly AppDbContext dbContext;
        private readonly ILogger<FeesController> _logger;
        private readonly IWebHostEnvironment _env;
        public FeesController(ILogger<FeesController> loggers, AppDbContext context, IWebHostEnvironment env)
        {
            _logger = loggers;
            dbContext = context;
            _env = env;
        }
        public IActionResult PaidFees()
        {
            GetMembers();
            var sessionUser = GetUserSession(HttpContext);
            ViewBag.ClientId = sessionUser.ClientId;
            return View();
        }

        public JsonResult GetMembers()
        {
            var sessionUser = GetUserSession(HttpContext);
            var list = dbContext.Members.Where(m => m.ClientId == sessionUser.ClientId && !m.IsDeleted && m.Status == "Active").Select(e => new { e.MemberId, Name = e.FullName + "(" + e.MemberId + ")" });
            ViewBag.List = list;
            return Json(list);
        }
        [HttpPost]
        public JsonResult FeesPaid(MemeberFee memeberFees)
        {
            var sessionUser = GetUserSession(HttpContext);

            // Normalize to month level (e.g., only one payment per month per member)
            var existingPayment = dbContext.MemeberFees.FirstOrDefault(m =>
                m.ClientId == sessionUser.ClientId &&
                m.MemberId == memeberFees.MemberId &&
                m.Date.Month == memeberFees.Date.Month &&
                m.Date.Year == memeberFees.Date.Year &&
                !m.IsDeleted);

            if (existingPayment != null)
            {
                // Already paid for this month
                return Json(new { success = false, message = "Already paid for this month." });
            }

            // Otherwise, add new payment
            memeberFees.ClientId = sessionUser.ClientId;
            memeberFees.IsDeleted = false;

            dbContext.MemeberFees.Add(memeberFees);
            dbContext.SaveChanges();

            return Json(new { success = true, message = "Payment recorded successfully." });
        }

    }
}

