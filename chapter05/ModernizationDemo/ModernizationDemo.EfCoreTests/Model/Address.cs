using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ModernizationDemo.EfCoreTests.Model;

[Index("CountryId", Name = "IX_Addresses_CountryId")]
public partial class Address
{
    [Key]
    public int Id { get; set; }

    [StringLength(200)]
    public string? Name { get; set; }

    [StringLength(200)]
    public string? Street { get; set; }

    [StringLength(200)]
    public string? City { get; set; }

    [Column("ZIP")]
    [StringLength(20)]
    public string? Zip { get; set; }

    [Column("IC")]
    [StringLength(20)]
    public string? Ic { get; set; }

    [Column("DIC")]
    [StringLength(20)]
    public string? Dic { get; set; }

    public int? CountryId { get; set; }

    [StringLength(200)]
    public string? CompanyRegistration { get; set; }

    [StringLength(50)]
    public string? BankAccount { get; set; }

    [StringLength(50)]
    public string? Iban { get; set; }

    [StringLength(50)]
    public string? Swift { get; set; }

    [ForeignKey("CountryId")]
    [InverseProperty("Addresses")]
    public virtual Country? Country { get; set; }

    [InverseProperty("Address")]
    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    [InverseProperty("Address")]
    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    [InverseProperty("BillingAddress")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [InverseProperty("Address")]
    public virtual ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();
}
