using System.ComponentModel.DataAnnotations;

namespace ModernizationDemo.App.Model;

public class AdminProductDetailModel
{
    [Required(ErrorMessage = "Product name is required!")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Product description is required!")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Product image is required!")]
    public string ImageUrl { get; set; }
}