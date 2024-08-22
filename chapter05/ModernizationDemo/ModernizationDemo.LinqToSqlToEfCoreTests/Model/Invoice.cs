using System;
using System.Collections.Generic;

namespace ModernizationDemo.LinqToSqlToEfCore.Model;

public partial class Invoice
{
    public int Id { get; set; }

    public string? Number { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime DueDate { get; set; }

    public DateTime? PaidDate { get; set; }

    public string? Notes { get; set; }

    public bool IsFinalInvoice { get; set; }

    public string? FileUrl { get; set; }

    public string? Currency { get; set; }

    public decimal TotalAmount { get; set; }

    public int SupplierId { get; set; }

    public int AddressId { get; set; }

    public int? CustomerId { get; set; }

    public bool IsCorrectionInvoice { get; set; }

    public string? CorrectedInvoiceNumber { get; set; }

    public decimal TotalAmountWithoutVat { get; set; }

    public DateTime? TaxDate { get; set; }

    public virtual ICollection<AccountTransaction> AccountTransactions { get; set; } = new List<AccountTransaction>();

    public virtual Address Address { get; set; } = null!;

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<InvoiceLine> InvoiceLines { get; set; } = new List<InvoiceLine>();

    public virtual ICollection<Order> OrderFinalInvoices { get; set; } = new List<Order>();

    public virtual ICollection<Order> OrderProformaInvoices { get; set; } = new List<Order>();

    public virtual Supplier Supplier { get; set; } = null!;
}
