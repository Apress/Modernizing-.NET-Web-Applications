using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModernizationDemo.EfCodeFirstTests.Model
{
    public partial class CourseReminder
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public int AppUserId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime ReminderDate { get; set; }

        public bool WasReminded { get; set; }

        public virtual Course Cours { get; set; }
    }
}
