using System;
using System.Collections.Generic;

namespace ModernizationDemo.LinqToSqlToEfCore.Model;

public partial class Supplier
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int AddressId { get; set; }

    public string? Currency { get; set; }

    public decimal Vatrate { get; set; }

    public bool IsVatpayer { get; set; }

    public bool IsAutomated { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual Address Address { get; set; } = null!;

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual ICollection<Lector> Lectors { get; set; } = new List<Lector>();
}
