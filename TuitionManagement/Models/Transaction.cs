using System;
using System.Collections.Generic;

namespace TuitionManagement.Models;

public partial class Transaction
{
    public string Id { get; set; } = null!;

    public string StudentId { get; set; } = null!;

    public string TuitionId { get; set; } = null!;

    public decimal? Amount { get; set; }

    public DateTime? TransactionDate { get; set; }

    public string? PaymentMethod { get; set; }

    public virtual Student Student { get; set; } = null!;

    public virtual Tuition Tuition { get; set; } = null!;
}
