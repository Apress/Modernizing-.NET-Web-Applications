using System.Collections.Generic;
using System.ServiceModel;
using ModernizationDemo.BusinessLogic.Models;

namespace ModernizationDemo.Wcf.Contracts
{
    [ServiceContract]
    public interface IProductService
    {
        [OperationContract]
        List<ProductModel> GetProducts();

        [OperationContract]
        ProductModel GetProduct(int id);

        [OperationContract]
        int AddProduct(ProductModel product);

        [OperationContract]
        void UpdateProduct(ProductModel product);

        [OperationContract]
        void RemoveProduct(int id);
    }
}
