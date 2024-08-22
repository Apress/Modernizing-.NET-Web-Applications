using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ModernizationDemo.EfCoreTests.Model;

[Index("AddressId", Name = "IX_Customers_AddressId")]
public partial class Customer
{
    [Key]
    public int Id { get; set; }

    public int AddressId { get; set; }

    [ForeignKey("AddressId")]
    [InverseProperty("Customers")]
    public virtual Address Address { get; set; } = null!;

    [InverseProperty("Customer")]
    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    [InverseProperty("Customer")]
    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    class EntityConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> entity)
        {
            entity.HasOne(d => d.Address).WithMany(p => p.Customers).OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
