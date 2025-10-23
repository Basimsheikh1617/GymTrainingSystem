using GymTrainingSystem.Data;
using GymTrainingSystem.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;

using static GymTrainingSystem.Helper.ApplicationHelper;

namespace GymTrainingSystem.Controllers
{
    public class MemberController : Controller
    {
        private readonly AppDbContext dbContext;
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _env;
        public MemberController(ILogger<HomeController> logger, AppDbContext context, IWebHostEnvironment env)
        {
            _logger = logger;
            dbContext = context;
            _env = env;
        }
        public IActionResult Members()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Get_Member()
        {
            var sessionUser = GetUserSession(HttpContext);
            
            List<SqlParameter> sqlParameters = new List<SqlParameter>
    {
        new SqlParameter("@clientId", sessionUser.ClientId)
    };

            DataSet dataset = ExecuteSp("Get_Members", sqlParameters.ToArray());

            if (dataset == null || dataset.Tables.Count == 0)
                return Json(new { data = new object[0] });

            var dt = dataset.Tables[0];
            var list = new List<Dictionary<string, object>>();

            foreach (DataRow row in dt.Rows)
            {
                var dict = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    dict[col.ColumnName] = row[col] == DBNull.Value ? null : row[col];
                }
                list.Add(dict);
            }

            return Json(new { data = list });
        }



    }
}
