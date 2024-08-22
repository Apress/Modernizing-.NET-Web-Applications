namespace ModernizationDemo.BusinessLogic.Models;

public class ProductModel
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public string ImageUrl { get; set; }

    public bool IsOnSale { get; set; }

    public string Unit { get; set; }

    public decimal UnitPrice { get; set; }

}