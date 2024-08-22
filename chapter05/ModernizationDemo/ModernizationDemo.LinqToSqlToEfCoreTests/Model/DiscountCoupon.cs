using System;
using System.Collections.Generic;

namespace ModernizationDemo.LinqToSqlToEfCore.Model;

public partial class DiscountCoupon
{
    public string? Code { get; set; }

    public decimal Percent { get; set; }

    public int? RestrictToCourseId { get; set; }

    public bool IsOneTime { get; set; }

    public DateTime? ExpirationDate { get; set; }

    public int Id { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Course? RestrictToCourse { get; set; }
}
