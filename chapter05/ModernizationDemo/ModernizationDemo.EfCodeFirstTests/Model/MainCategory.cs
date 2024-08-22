using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ModernizationDemo.EfCodeFirstTests.Model
{
    public partial class MainCategory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MainCategory()
        {
            Categories = new HashSet<Category>();
        }

        public int Id { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(200)]
        public string UrlName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Category> Categories { get; set; }
    }
}
