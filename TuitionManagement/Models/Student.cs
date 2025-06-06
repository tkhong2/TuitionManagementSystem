using System;
using System.Collections.Generic;

namespace TuitionManagement.Models;

public partial class Student
{
    public string Id { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Class { get; set; } = null!;

    public string Department { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual ICollection<Tuition> Tuitions { get; set; } = new List<Tuition>();
}
