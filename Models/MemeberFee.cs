using System;
using System.Collections.Generic;

namespace GymTrainingSystem.Models;

public partial class MemeberFee
{
    public int PaymentId { get; set; }

    public int ClientId { get; set; }

    public int MemberId { get; set; }

    public DateTime Date { get; set; }

    public string Status { get; set; } = null!;

    public decimal Fees { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? TimeStamp { get; set; }

    public virtual GymClient Client { get; set; } = null!;

    public virtual Member Member { get; set; } = null!;
}
