using System.ComponentModel.DataAnnotations;

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
         
        public class UserSessionModel
        {
            public int? UserId { get; set; }
            public string UserEmail { get; set; }
            public string UserName { get; set; }
            public int? ClientId { get; set; }
            public string ClientName { get; set; }
            public string Logo { get; set; }
            public string Address { get; set; }
        }
        public  class Member
        {

            public int ClientId { get; set; }

            [Required(ErrorMessage = "Required")]
            public string FullName { get; set; } = null!;

            [Required(ErrorMessage = "Required")]
            public string Gender { get; set; } = null!;

            [Required(ErrorMessage = "Required")]
            public int? Age { get; set; }
            [Required(ErrorMessage = "Required")]
            public long? PhoneNumber { get; set; }

            [Required(ErrorMessage = "Required")]
            public string? Email { get; set; }
            [Required(ErrorMessage = "Required")]
            public string? Address { get; set; }

            [Required(ErrorMessage = "Required")]
            public DateTime JoiningDate { get; set; }

            public string? MemberShipType { get; set; }


            [Required(ErrorMessage = "Required")]
            public bool PersonalTraining { get; set; }


            [Required(ErrorMessage = "Required")]
            public string Status { get; set; } = null!;
            [Required(ErrorMessage = "Required")]
            public decimal Fees { get; set; }


        }

    }
}
