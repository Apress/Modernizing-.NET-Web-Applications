using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModernizationDemo.EfCodeFirstTests.Model
{
    public partial class Invoice
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Invoice()
        {
            AccountTransactions = new HashSet<AccountTransaction>();
            InvoiceLines = new HashSet<InvoiceLine>();
            Orders = new HashSet<Order>();
            Orders1 = new HashSet<Order>();
        }

        public int Id { get; set; }

        [StringLength(20)]
        public string Number { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DueDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? PaidDate { get; set; }

        public string Notes { get; set; }

        public bool IsFinalInvoice { get; set; }

        [StringLength(200)]
        public string FileUrl { get; set; }

        public string Currency { get; set; }

        public decimal TotalAmount { get; set; }

        public int SupplierId { get; set; }

        public int AddressId { get; set; }

        public int? CustomerId { get; set; }

        public bool IsCorrectionInvoice { get; set; }

        [StringLength(20)]
        public string CorrectedInvoiceNumber { get; set; }

        public decimal TotalAmountWithoutVAT { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? TaxDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AccountTransaction> AccountTransactions { get; set; }

        public virtual Address Address { get; set; }

        public virtual Customer Customer { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvoiceLine> InvoiceLines { get; set; }

        public virtual Supplier Supplier { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders1 { get; set; }
    }
}
