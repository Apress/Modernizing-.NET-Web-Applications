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
    
    public partial class CourseReminder
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int AppUserId { get; set; }
        public System.DateTime ReminderDate { get; set; }
        public bool WasReminded { get; set; }
    
        public virtual Course Cours { get; set; }
    }
}
