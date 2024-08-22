using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ModernizationDemo.EfCoreTests.Model;

[Index("AddressId", Name = "IX_Invoices_AddressId")]
[Index("CustomerId", Name = "IX_Invoices_CustomerId")]
[Index("SupplierId", Name = "IX_Invoices_SupplierId")]
public partial class Invoice
{
    [Key]
    public int Id { get; set; }

    [StringLength(20)]
    public string? Number { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime DueDate { get; set; }

    public DateTime? PaidDate { get; set; }

    public string? Notes { get; set; }

    public bool IsFinalInvoice { get; set; }

    [StringLength(200)]
    public string? FileUrl { get; set; }

    public string? Currency { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal TotalAmount { get; set; }

    public int SupplierId { get; set; }

    public int AddressId { get; set; }

    public int? CustomerId { get; set; }

    public bool IsCorrectionInvoice { get; set; }

    [StringLength(20)]
    public string? CorrectedInvoiceNumber { get; set; }

    [Column("TotalAmountWithoutVAT", TypeName = "decimal(18, 2)")]
    public decimal TotalAmountWithoutVat { get; set; }

    public DateTime? TaxDate { get; set; }

    [InverseProperty("Invoice")]
    public virtual ICollection<AccountTransaction> AccountTransactions { get; set; } = new List<AccountTransaction>();

    [ForeignKey("AddressId")]
    [InverseProperty("Invoices")]
    public virtual Address Address { get; set; } = null!;

    [ForeignKey("CustomerId")]
    [InverseProperty("Invoices")]
    public virtual Customer? Customer { get; set; }

    [InverseProperty("Invoice")]
    public virtual ICollection<InvoiceLine> InvoiceLines { get; set; } = new List<InvoiceLine>();

    [InverseProperty("FinalInvoice")]
    public virtual ICollection<Order> OrderFinalInvoices { get; set; } = new List<Order>();

    [InverseProperty("ProformaInvoice")]
    public virtual ICollection<Order> OrderProformaInvoices { get; set; } = new List<Order>();

    [ForeignKey("SupplierId")]
    [InverseProperty("Invoices")]
    public virtual Supplier Supplier { get; set; } = null!;

    class EntityConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> entity)
        {
            entity.HasOne(d => d.Address).WithMany(p => p.Invoices).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Supplier).WithMany(p => p.Invoices).OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
