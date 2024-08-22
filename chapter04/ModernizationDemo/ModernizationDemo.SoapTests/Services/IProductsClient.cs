using ModernizationDemo.SoapTests.ProductsClient;

namespace ModernizationDemo.SoapTests.Services
{
    public interface IProductsClient
    {
        ProductModel[] GetProducts();
        ProductModel GetProduct(int id);
        int AddProduct(ProductModel product);
        void UpdateProduct(ProductModel product);
        void RemoveProduct(int id);
    }
}
