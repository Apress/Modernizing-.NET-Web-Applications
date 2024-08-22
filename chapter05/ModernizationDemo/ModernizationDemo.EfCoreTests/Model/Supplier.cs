using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ModernizationDemo.EfCoreTests.Model;

[Index("AddressId", Name = "IX_Suppliers_AddressId")]
public partial class Supplier
{
    [Key]
    public int Id { get; set; }

    [StringLength(200)]
    public string? Name { get; set; }

    public int AddressId { get; set; }

    [StringLength(20)]
    public string? Currency { get; set; }

    [Column("VATRate", TypeName = "decimal(18, 2)")]
    public decimal Vatrate { get; set; }

    [Column("IsVATPayer")]
    public bool IsVatpayer { get; set; }

    public bool IsAutomated { get; set; }

    [InverseProperty("Supplier")]
    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    [ForeignKey("AddressId")]
    [InverseProperty("Suppliers")]
    public virtual Address Address { get; set; } = null!;

    [InverseProperty("Supplier")]
    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    [InverseProperty("Supplier")]
    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    [InverseProperty("DefaultSupplier")]
    public virtual ICollection<Lector> Lectors { get; set; } = new List<Lector>();
}
