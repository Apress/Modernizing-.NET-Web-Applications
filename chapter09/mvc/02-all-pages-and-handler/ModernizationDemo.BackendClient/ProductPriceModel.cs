using System.ComponentModel.DataAnnotations;

namespace ModernizationDemo.BackendClient
{
    public partial class ProductPriceModel
    {
        public class Metadata
        {
            [Required(ErrorMessage = "Price is required!")]
            public double Price { get; set; }
        }
    }
}
