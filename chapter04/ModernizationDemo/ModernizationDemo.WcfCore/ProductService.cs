using ModernizationDemo.BusinessLogic.Models;
using ModernizationDemo.Wcf.Contracts;

namespace ModernizationDemo.WcfCore
{
    public class ProductService : IProductService
    {
        private readonly BusinessLogic.Services.ProductService service = new BusinessLogic.Services.ProductService();

        public List<ProductModel> GetProducts()
        {
            return service.GetProducts();
        }

        public ProductModel GetProduct(int id)
        {
            return service.GetProduct(id);
        }

        public int AddProduct(ProductModel product)
        {
            return service.AddProduct(product);
        }

        public void UpdateProduct(ProductModel product)
        {
            service.UpdateProduct(product);
        }

        public void RemoveProduct(int id)
        {
            service.RemoveProduct(id);
        }
    }
}
