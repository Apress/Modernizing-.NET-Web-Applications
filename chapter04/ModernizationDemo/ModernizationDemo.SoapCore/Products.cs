using System.ServiceModel;
using ModernizationDemo.BusinessLogic.Models;
using ModernizationDemo.BusinessLogic.Services;

namespace ModernizationDemo.SoapCore;

[ServiceContract]
public class Products
{
    private readonly ProductService service = new ProductService();

    [OperationContract]
    public List<ProductModel> GetProducts()
    {
        return service.GetProducts();
    }

    [OperationContract]
    public ProductModel GetProduct(int id)
    {
        return service.GetProduct(id);
    }

    [OperationContract]
    public int AddProduct(ProductModel product)
    {
        return service.AddProduct(product);
    }

    [OperationContract]
    public void UpdateProduct(ProductModel product)
    {
        service.UpdateProduct(product);
    }

    [OperationContract]
    public void RemoveProduct(int id)
    {
        service.RemoveProduct(id);
    }
}
