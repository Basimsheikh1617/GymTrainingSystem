using GymTrainingSystem.Data;
using GymTrainingSystem.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;
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
        public JsonResult DayWiseDashboard(int? clientId, DateTime Date)
        {
            DataSet dataset = new DataSet();
            Date = Date == DateTime.MinValue ? DateTime.Now : Date.Date;

            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@clientId",clientId),
                new SqlParameter("@Date",Date),

            };

            dataset = ExecuteSp("[Get_Dashboard]", sqlParameters.ToArray());
           
           
            return Json(JsonConvert.SerializeObject(dataset));
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

        public static DataSet ExecuteSp(string spName, SqlParameter[] sqlParams)
        {
            DataSet ds = new DataSet();
            using (SqlConnection sql = new SqlConnection("Server=DESKTOP-EE3AP9K;Database=GymSystem;Trusted_Connection=True;TrustServerCertificate=True;"))
            {
                using (SqlDataAdapter da = new SqlDataAdapter())
                {
                    using (SqlCommand cmd = new SqlCommand(spName, sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        //   cmd.Parameters.Add(new SqlParameter("@Id", Id));
                        cmd.Parameters.AddRange(sqlParams);
                        da.SelectCommand = cmd;
                        da.Fill(ds);
                        cmd.Parameters.Clear();
                        //sql.Open();
                    }
                }
            }
            return ds;
        }
    }
}
