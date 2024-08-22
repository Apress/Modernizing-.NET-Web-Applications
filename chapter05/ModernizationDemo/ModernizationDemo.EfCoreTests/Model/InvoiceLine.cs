using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ModernizationDemo.EfCoreTests.Model;

[Index("InvoiceId", Name = "IX_InvoiceLines_InvoiceId")]
public partial class InvoiceLine
{
    [Key]
    public int Id { get; set; }

    public int InvoiceId { get; set; }

    public string? Text { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal UnitPrice { get; set; }

    [Column("VATRate", TypeName = "decimal(18, 2)")]
    public decimal Vatrate { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Units { get; set; }

    public string? UnitName { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal TotalAmount { get; set; }

    [ForeignKey("InvoiceId")]
    [InverseProperty("InvoiceLines")]
    public virtual Invoice Invoice { get; set; } = null!;
}
