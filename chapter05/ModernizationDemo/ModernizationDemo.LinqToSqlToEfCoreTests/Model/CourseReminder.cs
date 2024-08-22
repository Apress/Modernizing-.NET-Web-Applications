using System;
using System.Collections.Generic;

namespace ModernizationDemo.LinqToSqlToEfCore.Model;

public partial class CourseReminder
{
    public int Id { get; set; }

    public int CourseId { get; set; }

    public int AppUserId { get; set; }

    public DateTime ReminderDate { get; set; }

    public bool WasReminded { get; set; }

    public virtual Course Course { get; set; } = null!;
}
