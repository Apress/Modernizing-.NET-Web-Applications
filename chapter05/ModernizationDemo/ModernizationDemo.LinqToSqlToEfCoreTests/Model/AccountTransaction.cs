using System;
using System.Collections.Generic;

namespace ModernizationDemo.LinqToSqlToEfCore.Model;

public partial class AccountTransaction
{
    public int Id { get; set; }

    public int AccountId { get; set; }

    public decimal? Amount { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? PaidDate { get; set; }

    public int? CourseId { get; set; }

    public string? LectorInvoiceUrl { get; set; }

    public string Discriminator { get; set; } = null!;

    public int? InvoiceId { get; set; }

    public string? Description { get; set; }

    public string? InvoiceUrl { get; set; }

    public decimal? CustomAccountTransactionAmount { get; set; }

    public DateTime? CustomAccountTransactionCreatedDate { get; set; }

    public DateTime? CustomAccountTransactionPaidDate { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Course? Course { get; set; }

    public virtual Invoice? Invoice { get; set; }
}
