using System;
using System.Collections.Generic;

namespace ModernizationDemo.LinqToSqlToEfCore.Model;

public partial class Course
{
    public int Id { get; set; }

    public DateTime BeginDate { get; set; }

    public bool IsEveningCourse { get; set; }

    public int NumberOfDays { get; set; }

    public int CourseTemplateId { get; set; }

    public string? LocationNotes { get; set; }

    public string? ContentNotes { get; set; }

    public int Type { get; set; }

    public int? LocationId { get; set; }

    public decimal Price { get; set; }

    public bool IsApproved { get; set; }

    public bool IsDeleted { get; set; }

    public int SupplierId { get; set; }

    public string? CourseSubtitle { get; set; }

    public bool IsRegistrationClosed { get; set; }

    public bool AllowOnlinePayments { get; set; }

    public bool AllowCashPayments { get; set; }

    public DateTime? ClosedDate { get; set; }

    public int? CustomerId { get; set; }

    public decimal Margin { get; set; }

    public decimal PricePerDay { get; set; }

    public int MinimumNumberOfAttendees { get; set; }

    public virtual ICollection<AccountTransaction> AccountTransactions { get; set; } = new List<AccountTransaction>();

    public virtual ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();

    public virtual ICollection<CourseDate> CourseDates { get; set; } = new List<CourseDate>();

    public virtual ICollection<CourseReminder> CourseReminders { get; set; } = new List<CourseReminder>();

    public virtual CourseTemplate CourseTemplate { get; set; } = null!;

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<DiscountCoupon> DiscountCoupons { get; set; } = new List<DiscountCoupon>();

    public virtual Location? Location { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Recording> Recordings { get; set; } = new List<Recording>();

    public virtual Supplier Supplier { get; set; } = null!;

    public virtual ICollection<Lector> Lectors { get; set; } = new List<Lector>();
}
