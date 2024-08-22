using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModernizationDemo.EfCodeFirstTests.Model
{
    public partial class PrivateCourseRequest
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PrivateCourseRequest()
        {
            Lectors = new HashSet<Lector>();
        }

        public int Id { get; set; }

        public string Topic { get; set; }

        [StringLength(100)]
        public string CompanyName { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        public int NumberOfAttendees { get; set; }

        public string Dates { get; set; }

        public string Notes { get; set; }

        public int AppUserId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedDate { get; set; }

        public int CourseLength { get; set; }

        public bool GrantFinancing { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Lector> Lectors { get; set; }
    }
}
