using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ModernizationDemo.EfCodeFirstTests.Model
{
    public partial class CourseTemplate
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CourseTemplate()
        {
            Courses = new HashSet<Course>();
            CourseTemplateRelations = new HashSet<CourseTemplateRelation>();
            CourseTemplateRelations1 = new HashSet<CourseTemplateRelation>();
            Orders = new HashSet<Order>();
            Categories = new HashSet<Category>();
        }

        public int Id { get; set; }

        public bool IsDeleted { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        public string Description { get; set; }

        [StringLength(200)]
        public string RequiredKnowledge { get; set; }

        public int TypicalNumberOfDays { get; set; }

        public bool IsPrivate { get; set; }

        [StringLength(50)]
        public string Code { get; set; }

        [StringLength(200)]
        public string UrlName { get; set; }

        [StringLength(500)]
        public string Keywords { get; set; }

        public int? PromotionOrder { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Course> Courses { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CourseTemplateRelation> CourseTemplateRelations { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CourseTemplateRelation> CourseTemplateRelations1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Category> Categories { get; set; }
    }
}
