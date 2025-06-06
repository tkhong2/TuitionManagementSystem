using System;
using System.Collections.Generic;

namespace TuitionManagement.Models;

public partial class Tuition
{
    public string Id { get; set; } = null!;

    public string StudentId { get; set; } = null!;

    public string? Semester { get; set; }

    public decimal? AmountDue { get; set; }

    public decimal? AmountPaid { get; set; }

    public string? Status { get; set; }

    public virtual Student Student { get; set; } = null!;

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
