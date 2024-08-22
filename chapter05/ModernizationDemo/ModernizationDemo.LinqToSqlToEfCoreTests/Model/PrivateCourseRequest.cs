using System;
using System.Collections.Generic;

namespace ModernizationDemo.LinqToSqlToEfCore.Model;

public partial class PrivateCourseRequest
{
    public int Id { get; set; }

    public string? Topic { get; set; }

    public string? CompanyName { get; set; }

    public string? City { get; set; }

    public int NumberOfAttendees { get; set; }

    public string? Dates { get; set; }

    public string? Notes { get; set; }

    public int AppUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public int CourseLength { get; set; }

    public bool? GrantFinancing { get; set; }

    public virtual ICollection<Lector> Lectors { get; set; } = new List<Lector>();
}
