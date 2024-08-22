using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ModernizationDemo.EfCoreTests.Model;

[Index("BillingAddressId", Name = "IX_Orders_BillingAddressId")]
[Index("CourseId", Name = "IX_Orders_CourseId")]
[Index("CourseTemplateId", Name = "IX_Orders_CourseTemplateId")]
[Index("DiscountCouponId", Name = "IX_Orders_DiscountCouponId")]
[Index("FinalInvoiceId", Name = "IX_Orders_FinalInvoiceId")]
[Index("ProformaInvoiceId", Name = "IX_Orders_ProformaInvoiceId")]
[Index("UserId", Name = "IX_Orders_UserId")]
public partial class Order
{
    [Key]
    public int Id { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? UserId { get; set; }

    public string? Notes { get; set; }

    public int? BillingAddressId { get; set; }

    public int CourseId { get; set; }

    public int? CourseTemplateId { get; set; }

    public int? DiscountCouponId { get; set; }

    public string? ContactEmail { get; set; }

    public string? ContactPhone { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal TotalPrice { get; set; }

    [Column("VATRate", TypeName = "decimal(18, 2)")]
    public decimal Vatrate { get; set; }

    [Column("HasVAT")]
    public bool HasVat { get; set; }

    public DateTime? PaidDate { get; set; }

    [StringLength(20)]
    public string? Currency { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal UnitPrice { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal DiscountPercent { get; set; }

    public int? ProformaInvoiceId { get; set; }

    public int? FinalInvoiceId { get; set; }

    public DateTime? CanceledDate { get; set; }

    [StringLength(200)]
    public string? BraintreeTransactionId { get; set; }

    [InverseProperty("Order")]
    public virtual ICollection<AttendeeRegistration> AttendeeRegistrations { get; set; } = new List<AttendeeRegistration>();

    [ForeignKey("BillingAddressId")]
    [InverseProperty("Orders")]
    public virtual Address? BillingAddress { get; set; }

    [ForeignKey("CourseId")]
    [InverseProperty("Orders")]
    public virtual Course Course { get; set; } = null!;

    [ForeignKey("CourseTemplateId")]
    [InverseProperty("Orders")]
    public virtual CourseTemplate? CourseTemplate { get; set; }

    [ForeignKey("DiscountCouponId")]
    [InverseProperty("Orders")]
    public virtual DiscountCoupon? DiscountCoupon { get; set; }

    [ForeignKey("FinalInvoiceId")]
    [InverseProperty("OrderFinalInvoices")]
    public virtual Invoice? FinalInvoice { get; set; }

    [InverseProperty("Order")]
    public virtual ICollection<OrderAvailableDate> OrderAvailableDates { get; set; } = new List<OrderAvailableDate>();

    [ForeignKey("ProformaInvoiceId")]
    [InverseProperty("OrderProformaInvoices")]
    public virtual Invoice? ProformaInvoice { get; set; }
}
