using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ModernizationDemo.EfCoreTests.Model;

[Index("OrderId", Name = "IX_AttendeeRegistrations_OrderId")]
[Index("UserId", Name = "IX_AttendeeRegistrations_UserId")]
public partial class AttendeeRegistration
{
    [Key]
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int OrderId { get; set; }

    public DateTime CreatedDate { get; set; }

    public bool IsDeleted { get; set; }

    [StringLength(200)]
    public string? FirstName { get; set; }

    [StringLength(200)]
    public string? LastName { get; set; }

    [StringLength(200)]
    public string? Email { get; set; }

    public bool IsUnknown { get; set; }

    public int? Day { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("AttendeeRegistrations")]
    public virtual Order Order { get; set; } = null!;
}
