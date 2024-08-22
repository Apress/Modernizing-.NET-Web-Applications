using System.Collections.Generic;
using System.Web.Services;
using ModernizationDemo.BusinessLogic.Models;
using ModernizationDemo.BusinessLogic.Services;

namespace ModernizationDemo.Soap
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class Products : System.Web.Services.WebService
    {
        private readonly ProductService service = new ProductService();

        [WebMethod]
        public List<ProductModel> GetProducts()
        {
            return service.GetProducts();
        }

        [WebMethod]
        public ProductModel GetProduct(int id)
        {
            return service.GetProduct(id);
        }

        [WebMethod]
        public int AddProduct(ProductModel product)
        {
            return service.AddProduct(product);
        }

        [WebMethod]
        public void UpdateProduct(ProductModel product)
        {
            service.UpdateProduct(product);
        }

        [WebMethod]
        public void RemoveProduct(int id)
        {
            service.RemoveProduct(id);
        }
    }
}
