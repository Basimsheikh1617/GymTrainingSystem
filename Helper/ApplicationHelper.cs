using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text.Json;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using static GymTrainingSystem.Models.MetaClass;

namespace GymTrainingSystem.Helper
{
    public static class ApplicationHelper
    {
        public static void SetUserSession(HttpContext httpContext, Models.User user)
        {
            var sessionUser = new UserSessionModel
            {
                UserId = user.Id,
                UserEmail = user.Email,
                UserName = user.Name,
                ClientId = user.ClientId,
                ClientName = user.Client?.Name,
                Logo = user.Client?.GymLogo,
                Address = user.Client?.Address
            };

            var userJson = JsonSerializer.Serialize(sessionUser);
            httpContext.Session.SetString("UserSession", userJson);
        }

        public static UserSessionModel? GetUserSession(HttpContext httpContext)
        {
            var userJson = httpContext.Session.GetString("UserSession");
            if (string.IsNullOrEmpty(userJson))
                return null;

            return JsonSerializer.Deserialize<UserSessionModel>(userJson);
        }

        public static void ClearSession(HttpContext httpContext)
        {
            httpContext.Session.Clear();
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
