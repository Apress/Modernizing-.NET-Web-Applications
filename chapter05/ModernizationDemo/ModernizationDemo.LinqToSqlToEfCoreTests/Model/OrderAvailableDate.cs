using System;
using System.Collections.Generic;

namespace ModernizationDemo.LinqToSqlToEfCore.Model;

public partial class OrderAvailableDate
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public DateTime Date { get; set; }

    public virtual Order Order { get; set; } = null!;
}
