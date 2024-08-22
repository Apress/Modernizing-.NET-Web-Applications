using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ModernizationDemo.EfCoreTests.Model;

[Index("AppUserId", Name = "IX_PrivateCourseRequests_AppUserId")]
public partial class PrivateCourseRequest
{
    [Key]
    public int Id { get; set; }

    public string? Topic { get; set; }

    [StringLength(100)]
    public string? CompanyName { get; set; }

    [StringLength(100)]
    public string? City { get; set; }

    public int NumberOfAttendees { get; set; }

    public string? Dates { get; set; }

    public string? Notes { get; set; }

    public int AppUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public int CourseLength { get; set; }

    [Required]
    public bool? GrantFinancing { get; set; }

    [ForeignKey("PrivateCourseRequestsId")]
    [InverseProperty("PrivateCourseRequests")]
    public virtual ICollection<Lector> Lectors { get; set; } = new List<Lector>();

    class EntityConfiguration : IEntityTypeConfiguration<PrivateCourseRequest>
    {
        public void Configure(EntityTypeBuilder<PrivateCourseRequest> entity)
        {
            entity.Property(e => e.GrantFinancing).HasDefaultValueSql("(CONVERT([bit],(0),(0)))");
        }
    }
}
