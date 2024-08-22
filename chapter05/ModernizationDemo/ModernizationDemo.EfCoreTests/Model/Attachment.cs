using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ModernizationDemo.EfCoreTests.Model;

[Index("CourseId", Name = "IX_Attachments_CourseId")]
public partial class Attachment
{
    [Key]
    public int Id { get; set; }

    [StringLength(200)]
    public string? Url { get; set; }

    public int CourseId { get; set; }

    [StringLength(200)]
    public string? FileName { get; set; }

    public string? Description { get; set; }

    [ForeignKey("CourseId")]
    [InverseProperty("Attachments")]
    public virtual Course Course { get; set; } = null!;
}
