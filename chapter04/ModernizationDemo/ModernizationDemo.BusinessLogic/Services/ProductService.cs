using System.Collections.Generic;
using System.Linq;
using ModernizationDemo.BusinessLogic.Exceptions;
using ModernizationDemo.BusinessLogic.Models;

namespace ModernizationDemo.BusinessLogic.Services;

// This is not a real-world implementation and is only used for demonstration purposes.
// The code is not thread-safe.
public class ProductService
{
    public List<ProductModel> GetProducts()
    {
        return Data.Products;
    }

    public ProductModel GetProduct(int id)
    {
        return Data.Products.SingleOrDefault(p => p.Id == id) 
               ?? throw new ItemNotFoundException();
    }

    public int AddProduct(ProductModel product)
    {
        product.Id = Data.Products.Max(p => p.Id) + 1;
        Data.Products.Add(product);
        return product.Id;
    }

    public void UpdateProduct(ProductModel product)
    {
        var index = Data.Products.FindIndex(p => p.Id == product.Id);
        if (index < 0)
        {
            throw new ItemNotFoundException();
        }
        Data.Products[index] = product;
    }

    public void RemoveProduct(int id)
    {
        GetProduct(id).IsOnSale = false;
    }
}
