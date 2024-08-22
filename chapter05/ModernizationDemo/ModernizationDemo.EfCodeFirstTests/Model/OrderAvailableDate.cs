using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModernizationDemo.EfCodeFirstTests.Model
{
    public partial class OrderAvailableDate
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime Date { get; set; }

        public virtual Order Order { get; set; }
    }
}
