using System;
using System.Collections.Generic;

namespace ModernizationDemo.LinqToSqlToEfCore.Model;

public partial class Lector
{
    public int Id { get; set; }

    public string? Bio { get; set; }

    public string? Blog { get; set; }

    public string? LinkedIn { get; set; }

    public string? Twitter { get; set; }

    public int UserId { get; set; }

    public string? UrlName { get; set; }

    public string? Website { get; set; }

    public int Order { get; set; }

    public string? AvatarUrl { get; set; }

    public bool IsDeleted { get; set; }

    public decimal CommissionPrice { get; set; }

    public decimal CommissionPercent { get; set; }

    public int? CommissionAccountId { get; set; }

    public int DefaultSupplierId { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual Account? CommissionAccount { get; set; }

    public virtual Supplier DefaultSupplier { get; set; } = null!;

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    public virtual ICollection<PrivateCourseRequest> PrivateCourseRequests { get; set; } = new List<PrivateCourseRequest>();
}
