using System;
using System.Collections.Generic;

namespace GymTrainingSystem.Models;

public partial class User
{
    public int Id { get; set; }

    public int? ClientId { get; set; }

    public string? Name { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public DateTime? TimeStamp { get; set; }

    public virtual GymClient? Client { get; set; }
}
