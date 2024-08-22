using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModernizationDemo.EfCodeFirstTests.Model
{
    public partial class AccountTransaction
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public decimal? Amount { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? CreatedDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? PaidDate { get; set; }

        public int? CourseId { get; set; }

        [StringLength(200)]
        public string LectorInvoiceUrl { get; set; }

        [Required]
        public string Discriminator { get; set; }

        public int? InvoiceId { get; set; }

        public string Description { get; set; }

        [StringLength(200)]
        public string InvoiceUrl { get; set; }

        public decimal? CustomAccountTransaction_Amount { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? CustomAccountTransaction_CreatedDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? CustomAccountTransaction_PaidDate { get; set; }

        public virtual Account Account { get; set; }

        public virtual Course Cours { get; set; }

        public virtual Invoice Invoice { get; set; }
    }
}
