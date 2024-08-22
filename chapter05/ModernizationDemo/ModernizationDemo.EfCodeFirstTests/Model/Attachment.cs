using System.ComponentModel.DataAnnotations;

namespace ModernizationDemo.EfCodeFirstTests.Model
{
    public partial class Attachment
    {
        public int Id { get; set; }

        [StringLength(200)]
        public string Url { get; set; }

        public int CourseId { get; set; }

        [StringLength(200)]
        public string FileName { get; set; }

        public string Description { get; set; }

        public virtual Course Cours { get; set; }
    }
}
