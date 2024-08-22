using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ModernizationDemo.EfCoreTests.Model;

[Index("CommissionAccountId", Name = "IX_Lectors_CommissionAccountId")]
[Index("DefaultSupplierId", Name = "IX_Lectors_DefaultSupplierId")]
[Index("UserId", Name = "IX_Lectors_UserId")]
public partial class Lector
{
    [Key]
    public int Id { get; set; }

    public string? Bio { get; set; }

    [StringLength(200)]
    public string? Blog { get; set; }

    [StringLength(200)]
    public string? LinkedIn { get; set; }

    [StringLength(200)]
    public string? Twitter { get; set; }

    public int UserId { get; set; }

    [StringLength(200)]
    public string? UrlName { get; set; }

    [StringLength(200)]
    public string? Website { get; set; }

    public int Order { get; set; }

    [StringLength(200)]
    public string? AvatarUrl { get; set; }

    public bool IsDeleted { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal CommissionPrice { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal CommissionPercent { get; set; }

    public int? CommissionAccountId { get; set; }

    public int DefaultSupplierId { get; set; }

    [InverseProperty("Lector")]
    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    [ForeignKey("CommissionAccountId")]
    [InverseProperty("Lectors")]
    public virtual Account? CommissionAccount { get; set; }

    [ForeignKey("DefaultSupplierId")]
    [InverseProperty("Lectors")]
    public virtual Supplier DefaultSupplier { get; set; } = null!;

    [ForeignKey("LectorsId")]
    [InverseProperty("Lectors")]
    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    [ForeignKey("LectorsId")]
    [InverseProperty("Lectors")]
    public virtual ICollection<PrivateCourseRequest> PrivateCourseRequests { get; set; } = new List<PrivateCourseRequest>();

    class EntityConfiguration : IEntityTypeConfiguration<Lector>
    {
        public void Configure(EntityTypeBuilder<Lector> entity)
        {
            entity.Property(e => e.DefaultSupplierId).HasDefaultValue(4);

            entity.HasOne(d => d.DefaultSupplier).WithMany(p => p.Lectors).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasMany(d => d.PrivateCourseRequests).WithMany(p => p.Lectors)
                .UsingEntity<Dictionary<string, object>>(
                    "LectorPrivateCourseRequest",
                    r => r.HasOne<PrivateCourseRequest>().WithMany().HasForeignKey("PrivateCourseRequestsId"),
                    l => l.HasOne<Lector>().WithMany()
                        .HasForeignKey("LectorsId")
                        .OnDelete(DeleteBehavior.ClientSetNull),
                    j =>
                    {
                        j.HasKey("LectorsId", "PrivateCourseRequestsId");
                        j.ToTable("LectorPrivateCourseRequest");
                        j.HasIndex(new[] { "PrivateCourseRequestsId" }, "IX_LectorPrivateCourseRequest_PrivateCourseRequestsId");
                    });
        }
    }
}
