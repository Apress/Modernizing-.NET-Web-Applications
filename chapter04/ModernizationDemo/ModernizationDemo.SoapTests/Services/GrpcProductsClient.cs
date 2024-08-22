using Grpc.Net.Client;
using ModernizationDemo.SoapGrpc;
using System.Linq;

namespace ModernizationDemo.SoapTests.Services
{
    public class GrpcProductsClient : IProductsClient
    {
        private readonly Products.ProductsClient client;

        public GrpcProductsClient(string url)
        {
            var channel = GrpcChannel.ForAddress(url);
            client = new Products.ProductsClient(channel);
        }

        public ProductsClient.ProductModel[] GetProducts()
        {
            return client.GetProducts(new GetProductsRequest()).Result
                .Select(ToProductModel)
                .ToArray();
        }

        public ProductsClient.ProductModel GetProduct(int id)
        {
            return ToProductModel(client.GetProduct(new GetProductRequest() { Id = id }).Result);
        }

        public int AddProduct(ProductsClient.ProductModel product)
        {
            return client.AddProduct(new AddProductRequest { Model = FromProductModel(product) }).Result;
        }

        public void UpdateProduct(ProductsClient.ProductModel product)
        {
            client.UpdateProduct(new UpdateProductRequest { Model = FromProductModel(product) });
        }

        public void RemoveProduct(int id)
        {
            client.RemoveProduct(new RemoveProductRequest() { Id = id });
        }

        private ProductsClient.ProductModel ToProductModel(ProductModel p)
        {
            return new ProductsClient.ProductModel
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                ImageUrl = p.ImageUrl,
                IsOnSale = p.IsOnSale,
                Unit = p.Unit,
                UnitPrice = (decimal)p.UnitPrice + 0.00m
            };
        }

        private ProductModel FromProductModel(ProductsClient.ProductModel p)
        {
            return new ProductModel
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                ImageUrl = p.ImageUrl,
                IsOnSale = p.IsOnSale,
                Unit = p.Unit,
                UnitPrice = (double)p.UnitPrice
            };
        }
    }
}
