using System;
using System.Collections.Generic;

namespace ModernizationDemo.LinqToSqlToEfCore.Model;

public partial class Customer
{
    public int Id { get; set; }

    public int AddressId { get; set; }

    public virtual Address Address { get; set; } = null!;

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}
