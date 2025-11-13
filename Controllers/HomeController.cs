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
    public class HomeController : Controller
    {
        private readonly AppDbContext dbContext;
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _env;
        public HomeController(ILogger<HomeController> logger, AppDbContext context, IWebHostEnvironment env)
        {
            _logger = logger;
            dbContext = context;
            _env = env;
        }

        public IActionResult Profile()
        {
            var sessionUser = GetUserSession(HttpContext);
            if (sessionUser.ClientId == null)
            {
                return RedirectToAction("Login", "Login");
            }

            // Fetch the client's data from database
            var client = dbContext.GymClients.FirstOrDefault(c => c.ClientId == sessionUser.ClientId);

            if (client == null)
            {
                return NotFound();
            }

            return View(client);
            
        }
        
        [HttpPost]
        public JsonResult SaveProfile(GymClient model, IFormFile? logo)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Invalid model data." });
            }

            var existingClient = dbContext.GymClients.FirstOrDefault(x => x.ClientId == model.ClientId);
            if (existingClient == null)
            {
                return Json(new { success = false, message = "Client not found." });
            }

            // Handle logo upload
            if (logo != null && logo.Length > 0)
            {
                string newPath = UploadClientLogo(logo);
                if (!string.IsNullOrEmpty(newPath))
                {
                    existingClient.GymLogo = newPath;
                }
            }

            // Update other fields
            existingClient.Name = model.Name;
            existingClient.Address = model.Address;

            dbContext.SaveChanges();

            // Directly update the current session model without re-fetching user from DB
            var sessionUser = GetUserSession(HttpContext);
            if (sessionUser != null)
            {
                sessionUser.ClientName = existingClient.Name;
                sessionUser.Address = existingClient.Address;
                sessionUser.Logo = existingClient.GymLogo ?? sessionUser.Logo;  // Keep old if no new logo

                // Fix serialization error (handle potential cycles or issues)
                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    WriteIndented = true  // Optional, for readability if debugging
                };
                var updatedJson = System.Text.Json.JsonSerializer.Serialize(sessionUser, options);
                HttpContext.Session.SetString("UserSession", updatedJson);
            }
            else
            {
                return Json(new { success = false, message = "Session error. Please log in again." });
            }

            return Json(new { success = true, message = "Profile updated successfully!" });
        }
        private string UploadClientLogo(IFormFile logo)
        {
            try
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(logo.FileName);
                string uploadPath = Path.Combine(_env.WebRootPath, "upload");

                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                string filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    logo.CopyTo(stream);
                }

                return Path.Combine("upload", fileName).Replace("\\", "/");
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult DayWiseDashboard(int? clientId, DateTime Date)
        {
            var sessionUser = GetUserSession(HttpContext);
            DataSet dataset = new DataSet();
            Date = Date == DateTime.MinValue ? DateTime.Now : Date.Date;
            var id = sessionUser.ClientId;
            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@clientId",sessionUser?.ClientId),
                new SqlParameter("@Date",Date),

            };

            dataset = ExecuteSp("[Get_Dashboard]", sqlParameters.ToArray());
           
           
            return Json(JsonConvert.SerializeObject(dataset));
        }
        public IActionResult Dashboard()
        {
            var sessionUser = GetUserSession(HttpContext);
            if (sessionUser == null)
            {
                return RedirectToAction("Index", "Login"); // agar session khatam ho gaya
            }
            return View();
        }
        public IActionResult Member()
        {
            var sessionUser = GetUserSession(HttpContext);

            if (sessionUser == null)
            {
                return RedirectToAction("Index", "Login"); // agar session khatam ho gaya
            }
            return View();
        }
        [HttpPost]
        public JsonResult AddMember([FromForm] Member member)
        {
            try
            {
                var sessionUser = GetUserSession(HttpContext);
                member.ClientId = sessionUser.ClientId;
                dbContext.Members.Add(member);
                dbContext.SaveChanges();
                return Json(new { success = true });
            }
            catch(Exception ex)
            {
                return Json(ex.InnerException);
            }
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //public static DataSet ExecuteSp(string spName, SqlParameter[] sqlParams)
        //{
        //    DataSet ds = new DataSet();
        //    using (SqlConnection sql = new SqlConnection("Server=DESKTOP-EE3AP9K;Database=GymSystem;Trusted_Connection=True;TrustServerCertificate=True;"))
        //    {
        //        using (SqlDataAdapter da = new SqlDataAdapter())
        //        {
        //            using (SqlCommand cmd = new SqlCommand(spName, sql))
        //            {
        //                cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //                //   cmd.Parameters.Add(new SqlParameter("@Id", Id));
        //                cmd.Parameters.AddRange(sqlParams);
        //                da.SelectCommand = cmd;
        //                da.Fill(ds);
        //                cmd.Parameters.Clear();
        //                //sql.Open();
        //            }
        //        }
        //    }
        //    return ds;
        //}
    }
}
