using System;
using System.Collections.Generic;

namespace GymTrainingSystem.Models;

public partial class GymClient
{
    public int ClientId { get; set; }

    public string Name { get; set; } = null!;

    public DateTime Date { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? TimeStamp { get; set; }

    public virtual ICollection<Member> Members { get; set; } = new List<Member>();

    public virtual ICollection<MemeberFee> MemeberFees { get; set; } = new List<MemeberFee>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
