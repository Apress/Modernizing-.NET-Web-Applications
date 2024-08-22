using System;
using System.Collections.Generic;

namespace ModernizationDemo.LinqToSqlToEfCore.Model;

public partial class Account
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? SupplierId { get; set; }

    public int? LectorId { get; set; }

    public string Discriminator { get; set; } = null!;

    public virtual ICollection<AccountTransaction> AccountTransactions { get; set; } = new List<AccountTransaction>();

    public virtual Lector? Lector { get; set; }

    public virtual ICollection<Lector> Lectors { get; set; } = new List<Lector>();

    public virtual Supplier? Supplier { get; set; }
}
