using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ModernizationDemo.EfCoreTests.Model;

[Index("RestrictToCourseId", Name = "IX_DiscountCoupons_RestrictToCourseId")]
public partial class DiscountCoupon
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string? Code { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Percent { get; set; }

    public int? RestrictToCourseId { get; set; }

    public bool IsOneTime { get; set; }

    public DateTime? ExpirationDate { get; set; }

    [InverseProperty("DiscountCoupon")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [ForeignKey("RestrictToCourseId")]
    [InverseProperty("DiscountCoupons")]
    public virtual Course? RestrictToCourse { get; set; }
}
