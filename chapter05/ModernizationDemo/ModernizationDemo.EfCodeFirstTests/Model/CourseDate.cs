using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModernizationDemo.EfCodeFirstTests.Model
{
    public partial class CourseDate
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime BeginDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime EndDate { get; set; }

        public virtual Course Cours { get; set; }
    }
}
