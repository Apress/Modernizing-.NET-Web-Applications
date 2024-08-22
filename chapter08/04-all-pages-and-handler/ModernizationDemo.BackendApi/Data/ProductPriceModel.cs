namespace ModernizationDemo.BackendApi.Data;

public class ProductPriceModel
{
    public Guid ProductId { get; set; }

    public string CurrencyCode { get; set; }

    public decimal Price { get; set; }
}