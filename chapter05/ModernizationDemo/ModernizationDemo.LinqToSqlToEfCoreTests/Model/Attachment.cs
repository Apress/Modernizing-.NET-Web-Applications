using System;
using System.Collections.Generic;

namespace ModernizationDemo.LinqToSqlToEfCore.Model;

public partial class Attachment
{
    public int Id { get; set; }

    public string? Url { get; set; }

    public int CourseId { get; set; }

    public string? FileName { get; set; }

    public string? Description { get; set; }

    public virtual Course Course { get; set; } = null!;
}
