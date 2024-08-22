using System;
using System.Collections.Generic;

namespace ModernizationDemo.LinqToSqlToEfCore.Model;

public partial class Country
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
}
