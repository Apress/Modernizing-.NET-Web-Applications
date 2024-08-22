using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ModernizationDemo.EfCoreTests.Model;

[Index("CourseId", Name = "IX_CourseDates_CourseId")]
public partial class CourseDate
{
    [Key]
    public int Id { get; set; }

    public int CourseId { get; set; }

    public DateTime BeginDate { get; set; }

    public DateTime EndDate { get; set; }

    [ForeignKey("CourseId")]
    [InverseProperty("CourseDates")]
    public virtual Course Course { get; set; } = null!;
}
