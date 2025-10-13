using System;
using System.Collections.Generic;

namespace GymTrainingSystem.Models;

public partial class Member
{
    public int MemberId { get; set; }

    public int? ClientId { get; set; }

    public string FullName { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public int? Age { get; set; }

    public long? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public DateTime JoiningDate { get; set; }

    public string? MemberShipType { get; set; }

    public DateTime? MemberShipStart { get; set; }

    public DateTime? MemberShipEnd { get; set; }

    public bool PersonalTraining { get; set; }

    public bool IsDeleted { get; set; }

    public string Status { get; set; } = null!;

    public decimal? Fees { get; set; }

    public DateTime? TimeStamp { get; set; }

    public virtual GymClient? Client { get; set; }

    public virtual ICollection<MemeberFee> MemeberFees { get; set; } = new List<MemeberFee>();
}
