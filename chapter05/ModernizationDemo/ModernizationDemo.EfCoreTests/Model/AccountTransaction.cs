using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ModernizationDemo.EfCoreTests.Model;

[Index("AccountId", Name = "IX_AccountTransactions_AccountId")]
[Index("CourseId", Name = "IX_AccountTransactions_CourseId")]
[Index("InvoiceId", Name = "IX_AccountTransactions_InvoiceId")]
public partial class AccountTransaction
{
    [Key]
    public int Id { get; set; }

    public int AccountId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Amount { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? PaidDate { get; set; }

    public int? CourseId { get; set; }

    [StringLength(200)]
    public string? LectorInvoiceUrl { get; set; }

    public string Discriminator { get; set; } = null!;

    public int? InvoiceId { get; set; }

    public string? Description { get; set; }

    [StringLength(200)]
    public string? InvoiceUrl { get; set; }

    [Column("CustomAccountTransaction_Amount", TypeName = "decimal(18, 2)")]
    public decimal? CustomAccountTransactionAmount { get; set; }

    [Column("CustomAccountTransaction_CreatedDate")]
    public DateTime? CustomAccountTransactionCreatedDate { get; set; }

    [Column("CustomAccountTransaction_PaidDate")]
    public DateTime? CustomAccountTransactionPaidDate { get; set; }

    [ForeignKey("AccountId")]
    [InverseProperty("AccountTransactions")]
    public virtual Account Account { get; set; } = null!;

    [ForeignKey("CourseId")]
    [InverseProperty("AccountTransactions")]
    public virtual Course? Course { get; set; }

    [ForeignKey("InvoiceId")]
    [InverseProperty("AccountTransactions")]
    public virtual Invoice? Invoice { get; set; }

    class EntityConfiguration : IEntityTypeConfiguration<AccountTransaction>
    {
        public void Configure(EntityTypeBuilder<AccountTransaction> entity)
        {
            entity.HasOne(d => d.Course).WithMany(p => p.AccountTransactions).OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Invoice).WithMany(p => p.AccountTransactions).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
