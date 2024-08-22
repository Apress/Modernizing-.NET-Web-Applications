using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModernizationDemo.EfCodeFirstTests.Model
{
    public partial class DiscountCoupon
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DiscountCoupon()
        {
            Orders = new HashSet<Order>();
        }

        [StringLength(100)]
        public string Code { get; set; }

        public decimal Percent { get; set; }

        public int? RestrictToCourseId { get; set; }

        public bool IsOneTime { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ExpirationDate { get; set; }

        public int Id { get; set; }

        public virtual Course Cours { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }
    }
}
