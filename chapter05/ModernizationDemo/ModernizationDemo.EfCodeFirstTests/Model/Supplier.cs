using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ModernizationDemo.EfCodeFirstTests.Model
{
    public partial class Supplier
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Supplier()
        {
            Accounts = new HashSet<Account>();
            Courses = new HashSet<Course>();
            Invoices = new HashSet<Invoice>();
            Lectors = new HashSet<Lector>();
        }

        public int Id { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        public int AddressId { get; set; }

        [StringLength(20)]
        public string Currency { get; set; }

        public decimal VATRate { get; set; }

        public bool IsVATPayer { get; set; }

        public bool IsAutomated { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Account> Accounts { get; set; }

        public virtual Address Address { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Course> Courses { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Invoice> Invoices { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Lector> Lectors { get; set; }
    }
}
