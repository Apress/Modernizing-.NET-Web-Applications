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
    
    public partial class PrivateCourseRequest
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PrivateCourseRequest()
        {
            this.Lectors = new HashSet<Lector>();
        }
    
        public int Id { get; set; }
        public string Topic { get; set; }
        public string CompanyName { get; set; }
        public string City { get; set; }
        public int NumberOfAttendees { get; set; }
        public string Dates { get; set; }
        public string Notes { get; set; }
        public int AppUserId { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CourseLength { get; set; }
        public bool GrantFinancing { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Lector> Lectors { get; set; }
    }
}
