using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ModernizationDemo.EfCodeFirstTests.Model
{
    public partial class Category
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Category()
        {
            CourseTemplates = new HashSet<CourseTemplate>();
        }

        public int Id { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(200)]
        public string UrlName { get; set; }

        public string Description { get; set; }

        public int? PromotionOrder { get; set; }

        public int MainCategoryId { get; set; }

        public string ImageUrl { get; set; }

        public virtual MainCategory MainCategory { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CourseTemplate> CourseTemplates { get; set; }
    }
}
