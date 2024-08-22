using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ModernizationDemo.EfCodeFirstTests.Model
{
    public partial class Address
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Address()
        {
            Customers = new HashSet<Customer>();
            Invoices = new HashSet<Invoice>();
            Orders = new HashSet<Order>();
            Suppliers = new HashSet<Supplier>();
        }

        public int Id { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Street { get; set; }

        [StringLength(200)]
        public string City { get; set; }

        [StringLength(20)]
        public string ZIP { get; set; }

        [StringLength(20)]
        public string IC { get; set; }

        [StringLength(20)]
        public string DIC { get; set; }

        public int? CountryId { get; set; }

        [StringLength(200)]
        public string CompanyRegistration { get; set; }

        [StringLength(50)]
        public string BankAccount { get; set; }

        [StringLength(50)]
        public string Iban { get; set; }

        [StringLength(50)]
        public string Swift { get; set; }

        public virtual Country Country { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Customer> Customers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Invoice> Invoices { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Supplier> Suppliers { get; set; }
    }
}
