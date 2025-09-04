namespace GymTrainingSystem.Models
{
    public class MetaClass
    {

        public class UserSessionData
        {
            public int ID { get; set; }

            public int RoleId { get; set; }
            public string RoleName { get; set; }
            public string ClientId { get; set; }
            public string Name { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }
            public List<int> UsersOrgs { get; set; }
            public int SessionId { get; set; }
            public int CookieId { get; set; }
            public string LoginCode { get; set; }
            public string ProfileImage { get; set; }
            public string ProfileImageWithPath { get; set; }
            public int EmployeeId { get; set; }

            private bool _IsUserSetup = true;
            public bool IsUserSetup
            {
                get
                {
                    return _IsUserSetup;
                }
                set
                {
                    _IsUserSetup = value;
                }
            }
        }
    }
}
