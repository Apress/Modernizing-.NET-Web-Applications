using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ModernizationDemo.EfCoreTests.Model;

[Index("OrderId", Name = "IX_OrderAvailableDates_OrderId")]
public partial class OrderAvailableDate
{
    [Key]
    public int Id { get; set; }

    public int OrderId { get; set; }

    public DateTime Date { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("OrderAvailableDates")]
    public virtual Order Order { get; set; } = null!;
}
