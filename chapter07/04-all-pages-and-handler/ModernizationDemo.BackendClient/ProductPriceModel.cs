using System.ComponentModel.DataAnnotations;

namespace ModernizationDemo.BackendClient
{
    public partial class ProductPriceModel
    {
        class Metadata
        {
            [Required(ErrorMessage = "Price is required!")]
            public double Price { get; set; }
        }
    }
}
