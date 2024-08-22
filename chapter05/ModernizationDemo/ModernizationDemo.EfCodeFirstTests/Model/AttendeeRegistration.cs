using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModernizationDemo.EfCodeFirstTests.Model
{
    public partial class AttendeeRegistration
    {
        public int Id { get; set; }

        public int? UserId { get; set; }

        public int OrderId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedDate { get; set; }

        public bool IsDeleted { get; set; }

        [StringLength(200)]
        public string FirstName { get; set; }

        [StringLength(200)]
        public string LastName { get; set; }

        [StringLength(200)]
        public string Email { get; set; }

        public bool IsUnknown { get; set; }

        public int? Day { get; set; }

        public virtual Order Order { get; set; }
    }
}
