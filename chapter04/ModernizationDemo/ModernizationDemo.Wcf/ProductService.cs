using System.Collections.Generic;
using System.Security.Permissions;
using ModernizationDemo.BusinessLogic.Models;
using ModernizationDemo.Wcf.Contracts;

namespace ModernizationDemo.Wcf
{
    public class ProductService : IProductService
    {
        private readonly BusinessLogic.Services.ProductService service = new BusinessLogic.Services.ProductService();

        [PrincipalPermission(SecurityAction.Demand, Role = "admin", Authenticated = true)]
        public List<ProductModel> GetProducts()
        {
            return service.GetProducts();
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "admin", Authenticated = true)]
        public ProductModel GetProduct(int id)
        {
            return service.GetProduct(id);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "admin", Authenticated = true)]
        public int AddProduct(ProductModel product)
        {
            return service.AddProduct(product);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "admin", Authenticated = true)]
        public void UpdateProduct(ProductModel product)
        {
            service.UpdateProduct(product);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "admin", Authenticated = true)]
        public void RemoveProduct(int id)
        {
            service.RemoveProduct(id);
        }
    }
}