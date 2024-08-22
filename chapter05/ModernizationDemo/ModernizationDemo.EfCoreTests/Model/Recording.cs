using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ModernizationDemo.EfCoreTests.Model;

[Index("CourseId", Name = "IX_Recordings_CourseId")]
public partial class Recording
{
    [Key]
    public int Id { get; set; }

    public string? AzureAssetId { get; set; }

    public string? PublicLinkUrl { get; set; }

    public string? Description { get; set; }

    public int CourseId { get; set; }

    [ForeignKey("CourseId")]
    [InverseProperty("Recordings")]
    public virtual Course Course { get; set; } = null!;
}
