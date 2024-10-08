//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ModernizationDemo.EfTests.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class CourseTemplate
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CourseTemplate()
        {
            this.Courses = new HashSet<Course>();
            this.CourseTemplateRelations = new HashSet<CourseTemplateRelation>();
            this.CourseTemplateRelations1 = new HashSet<CourseTemplateRelation>();
            this.Orders = new HashSet<Order>();
            this.Categories = new HashSet<Category>();
        }
    
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string RequiredKnowledge { get; set; }
        public int TypicalNumberOfDays { get; set; }
        public bool IsPrivate { get; set; }
        public string Code { get; set; }
        public string UrlName { get; set; }
        public string Keywords { get; set; }
        public Nullable<int> PromotionOrder { get; set; }
    
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
