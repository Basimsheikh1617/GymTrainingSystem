using System.Text.Json;
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
                ClientName = user.Client?.Name
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
    }
}
