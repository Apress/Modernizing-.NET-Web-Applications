using System;
using System.Collections.Generic;

namespace ModernizationDemo.LinqToSqlToEfCore.Model;

public partial class AttendeeRegistration
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int OrderId { get; set; }

    public DateTime CreatedDate { get; set; }

    public bool IsDeleted { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public bool IsUnknown { get; set; }

    public int? Day { get; set; }

    public virtual Order Order { get; set; } = null!;
}
