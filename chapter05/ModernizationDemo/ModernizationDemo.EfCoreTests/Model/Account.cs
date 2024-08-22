using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ModernizationDemo.EfCoreTests.Model;

[Index("LectorId", Name = "IX_Accounts_LectorId")]
[Index("SupplierId", Name = "IX_Accounts_SupplierId")]
public partial class Account
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string? Name { get; set; }

    public int? SupplierId { get; set; }

    public int? LectorId { get; set; }

    public string Discriminator { get; set; } = null!;

    [InverseProperty("Account")]
    public virtual ICollection<AccountTransaction> AccountTransactions { get; set; } = new List<AccountTransaction>();

    [ForeignKey("LectorId")]
    [InverseProperty("Accounts")]
    public virtual Lector? Lector { get; set; }

    [InverseProperty("CommissionAccount")]
    public virtual ICollection<Lector> Lectors { get; set; } = new List<Lector>();

    [ForeignKey("SupplierId")]
    [InverseProperty("Accounts")]
    public virtual Supplier? Supplier { get; set; }
}
