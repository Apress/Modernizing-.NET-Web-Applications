using System;
using System.Collections.Generic;

namespace ModernizationDemo.LinqToSqlToEfCore.Model;

public partial class Order
{
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

    public decimal TotalPrice { get; set; }

    public decimal Vatrate { get; set; }

    public bool HasVat { get; set; }

    public DateTime? PaidDate { get; set; }

    public string? Currency { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal DiscountPercent { get; set; }

    public int? ProformaInvoiceId { get; set; }

    public int? FinalInvoiceId { get; set; }

    public DateTime? CanceledDate { get; set; }

    public string? BraintreeTransactionId { get; set; }

    public virtual ICollection<AttendeeRegistration> AttendeeRegistrations { get; set; } = new List<AttendeeRegistration>();

    public virtual Address? BillingAddress { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual CourseTemplate? CourseTemplate { get; set; }

    public virtual DiscountCoupon? DiscountCoupon { get; set; }

    public virtual Invoice? FinalInvoice { get; set; }

    public virtual ICollection<OrderAvailableDate> OrderAvailableDates { get; set; } = new List<OrderAvailableDate>();

    public virtual Invoice? ProformaInvoice { get; set; }
}
