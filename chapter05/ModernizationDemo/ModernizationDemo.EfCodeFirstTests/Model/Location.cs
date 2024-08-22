using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ModernizationDemo.EfCodeFirstTests.Model
{
    public partial class Location
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Location()
        {
            Courses = new HashSet<Course>();
        }

        public int Id { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        [StringLength(200)]
        public string City { get; set; }

        [StringLength(200)]
        public string ZIP { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string HowToGetThere { get; set; }

        public int GeekcoreLocationId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Course> Courses { get; set; }
    }
}
