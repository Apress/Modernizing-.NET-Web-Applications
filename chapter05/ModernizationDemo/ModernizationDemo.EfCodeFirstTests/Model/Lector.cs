using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ModernizationDemo.EfCodeFirstTests.Model
{
    public partial class Lector
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Lector()
        {
            Accounts = new HashSet<Account>();
            Courses = new HashSet<Course>();
            PrivateCourseRequests = new HashSet<PrivateCourseRequest>();
        }

        public int Id { get; set; }

        public string Bio { get; set; }

        [StringLength(200)]
        public string Blog { get; set; }

        [StringLength(200)]
        public string LinkedIn { get; set; }

        [StringLength(200)]
        public string Twitter { get; set; }

        public int UserId { get; set; }

        [StringLength(200)]
        public string UrlName { get; set; }

        [StringLength(200)]
        public string Website { get; set; }

        public int Order { get; set; }

        [StringLength(200)]
        public string AvatarUrl { get; set; }

        public bool IsDeleted { get; set; }

        public decimal CommissionPrice { get; set; }

        public decimal CommissionPercent { get; set; }

        public int? CommissionAccountId { get; set; }

        public int DefaultSupplierId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Account> Accounts { get; set; }

        public virtual Account Account { get; set; }

        public virtual Supplier Supplier { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Course> Courses { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PrivateCourseRequest> PrivateCourseRequests { get; set; }
    }
}
