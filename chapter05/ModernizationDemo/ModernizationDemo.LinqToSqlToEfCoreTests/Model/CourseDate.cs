using System;
using System.Collections.Generic;

namespace ModernizationDemo.LinqToSqlToEfCore.Model;

public partial class CourseDate
{
    public int Id { get; set; }

    public int CourseId { get; set; }

    public DateTime BeginDate { get; set; }

    public DateTime EndDate { get; set; }

    public virtual Course Course { get; set; } = null!;
}
