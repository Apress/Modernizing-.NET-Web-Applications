using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModernizationDemo.EfCodeFirstTests.Model
{
    [Table("Courses")]
    public partial class Course
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Course()
        {
            AccountTransactions = new HashSet<AccountTransaction>();
            Attachments = new HashSet<Attachment>();
            CourseDates = new HashSet<CourseDate>();
            CourseReminders = new HashSet<CourseReminder>();
            DiscountCoupons = new HashSet<DiscountCoupon>();
            Orders = new HashSet<Order>();
            Recordings = new HashSet<Recording>();
            Lectors = new HashSet<Lector>();
        }

        public int Id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime BeginDate { get; set; }

        public bool IsEveningCourse { get; set; }

        public int NumberOfDays { get; set; }

        public int CourseTemplateId { get; set; }

        public string LocationNotes { get; set; }

        public string ContentNotes { get; set; }

        public int Type { get; set; }

        public int? LocationId { get; set; }

        public decimal Price { get; set; }

        public bool IsApproved { get; set; }

        public bool IsDeleted { get; set; }

        public int SupplierId { get; set; }

        [StringLength(200)]
        public string CourseSubtitle { get; set; }

        public bool IsRegistrationClosed { get; set; }

        public bool AllowOnlinePayments { get; set; }

        public bool AllowCashPayments { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ClosedDate { get; set; }

        public int? CustomerId { get; set; }

        public decimal Margin { get; set; }

        public decimal PricePerDay { get; set; }

        public int MinimumNumberOfAttendees { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AccountTransaction> AccountTransactions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Attachment> Attachments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CourseDate> CourseDates { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CourseReminder> CourseReminders { get; set; }

        public virtual CourseTemplate CourseTemplate { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual Location Location { get; set; }

        public virtual Supplier Supplier { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DiscountCoupon> DiscountCoupons { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Recording> Recordings { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Lector> Lectors { get; set; }
    }
}
