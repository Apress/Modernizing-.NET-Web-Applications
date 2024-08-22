using ModernizationDemo.SoapTests.ProductsClient;

namespace ModernizationDemo.SoapTests.Services
{
    public class SoapProductsClient : IProductsClient
    {
        private readonly Products client;

        public SoapProductsClient(string url)
        {
            client = new ProductsClient.Products() { Url = url };
        }

        public ProductModel[] GetProducts()
        {
            return client.GetProducts();
        }

        public ProductModel GetProduct(int id)
        {
            return client.GetProduct(id);
        }

        public int AddProduct(ProductModel product)
        {
            return client.AddProduct(product);
        }

        public void UpdateProduct(ProductModel product)
        {
            client.UpdateProduct(product);
        }

        public void RemoveProduct(int id)
        {
            client.RemoveProduct(id);
        }
    }
}
