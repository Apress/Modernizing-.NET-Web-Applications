using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ModernizationDemo.EfCoreTests.Model;

[Index("AppUserId", Name = "IX_CourseReminders_AppUserId")]
[Index("CourseId", Name = "IX_CourseReminders_CourseId")]
public partial class CourseReminder
{
    [Key]
    public int Id { get; set; }

    public int CourseId { get; set; }

    public int AppUserId { get; set; }

    public DateTime ReminderDate { get; set; }

    public bool WasReminded { get; set; }

    [ForeignKey("CourseId")]
    [InverseProperty("CourseReminders")]
    public virtual Course Course { get; set; } = null!;
}
