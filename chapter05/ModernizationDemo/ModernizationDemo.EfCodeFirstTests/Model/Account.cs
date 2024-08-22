using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ModernizationDemo.EfCodeFirstTests.Model
{
    public partial class Account
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Account()
        {
            AccountTransactions = new HashSet<AccountTransaction>();
            Lectors = new HashSet<Lector>();
        }

        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        public int? SupplierId { get; set; }

        public int? LectorId { get; set; }

        [Required]
        public string Discriminator { get; set; }

        public virtual Lector Lector { get; set; }

        public virtual Supplier Supplier { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AccountTransaction> AccountTransactions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Lector> Lectors { get; set; }
    }
}
