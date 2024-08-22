using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ModernizationDemo.EfCoreTests.Model;

[Index("CourseTemplateId", Name = "IX_Courses_CourseTemplateId")]
[Index("CustomerId", Name = "IX_Courses_CustomerId")]
[Index("LocationId", Name = "IX_Courses_LocationId")]
[Index("SupplierId", Name = "IX_Courses_SupplierId")]
public partial class Course
{
    [Key]
    public int Id { get; set; }

    public DateTime BeginDate { get; set; }

    public bool IsEveningCourse { get; set; }

    public int NumberOfDays { get; set; }

    public int CourseTemplateId { get; set; }

    public string? LocationNotes { get; set; }

    public string? ContentNotes { get; set; }

    public int Type { get; set; }

    public int? LocationId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }

    public bool IsApproved { get; set; }

    public bool IsDeleted { get; set; }

    public int SupplierId { get; set; }

    [StringLength(200)]
    public string? CourseSubtitle { get; set; }

    public bool IsRegistrationClosed { get; set; }

    public bool AllowOnlinePayments { get; set; }

    public bool AllowCashPayments { get; set; }

    public DateTime? ClosedDate { get; set; }

    public int? CustomerId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Margin { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal PricePerDay { get; set; }

    public int MinimumNumberOfAttendees { get; set; }

    [InverseProperty("Course")]
    public virtual ICollection<AccountTransaction> AccountTransactions { get; set; } = new List<AccountTransaction>();

    [InverseProperty("Course")]
    public virtual ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();

    [InverseProperty("Course")]
    public virtual ICollection<CourseDate> CourseDates { get; set; } = new List<CourseDate>();

    [InverseProperty("Course")]
    public virtual ICollection<CourseReminder> CourseReminders { get; set; } = new List<CourseReminder>();

    [ForeignKey("CourseTemplateId")]
    [InverseProperty("Courses")]
    public virtual CourseTemplate CourseTemplate { get; set; } = null!;

    [ForeignKey("CustomerId")]
    [InverseProperty("Courses")]
    public virtual Customer? Customer { get; set; }

    [InverseProperty("RestrictToCourse")]
    public virtual ICollection<DiscountCoupon> DiscountCoupons { get; set; } = new List<DiscountCoupon>();

    [ForeignKey("LocationId")]
    [InverseProperty("Courses")]
    public virtual Location? Location { get; set; }

    [InverseProperty("Course")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [InverseProperty("Course")]
    public virtual ICollection<Recording> Recordings { get; set; } = new List<Recording>();

    [ForeignKey("SupplierId")]
    [InverseProperty("Courses")]
    public virtual Supplier Supplier { get; set; } = null!;

    [ForeignKey("CoursesId")]
    [InverseProperty("Courses")]
    public virtual ICollection<Lector> Lectors { get; set; } = new List<Lector>();

    class EntityConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> entity)
        {
            entity.HasMany(d => d.Lectors).WithMany(p => p.Courses)
                .UsingEntity<Dictionary<string, object>>(
                    "CourseLector",
                    r => r.HasOne<Lector>().WithMany().HasForeignKey("LectorsId"),
                    l => l.HasOne<Course>().WithMany().HasForeignKey("CoursesId"),
                    j =>
                    {
                        j.HasKey("CoursesId", "LectorsId");
                        j.ToTable("CourseLector");
                        j.HasIndex(new[] { "LectorsId" }, "IX_CourseLector_LectorsId");
                    });
        }
    }
}
