using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModernizationDemo.EfCodeFirstTests.Model
{
    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            AttendeeRegistrations = new HashSet<AttendeeRegistration>();
            OrderAvailableDates = new HashSet<OrderAvailableDate>();
        }

        public int Id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedDate { get; set; }

        public int? UserId { get; set; }

        public string Notes { get; set; }

        public int? BillingAddressId { get; set; }

        public int CourseId { get; set; }

        public int? CourseTemplateId { get; set; }

        public int? DiscountCouponId { get; set; }

        public string ContactEmail { get; set; }

        public string ContactPhone { get; set; }

        public decimal TotalPrice { get; set; }

        public decimal VATRate { get; set; }

        public bool HasVAT { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? PaidDate { get; set; }

        [StringLength(20)]
        public string Currency { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal DiscountPercent { get; set; }

        public int? ProformaInvoiceId { get; set; }

        public int? FinalInvoiceId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? CanceledDate { get; set; }

        [StringLength(200)]
        public string BraintreeTransactionId { get; set; }

        public virtual Address Address { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AttendeeRegistration> AttendeeRegistrations { get; set; }

        public virtual Course Cours { get; set; }

        public virtual CourseTemplate CourseTemplate { get; set; }

        public virtual DiscountCoupon DiscountCoupon { get; set; }

        public virtual Invoice Invoice { get; set; }

        public virtual Invoice Invoice1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderAvailableDate> OrderAvailableDates { get; set; }
    }
}
