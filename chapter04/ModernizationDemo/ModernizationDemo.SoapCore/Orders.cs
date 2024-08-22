using System.ServiceModel;
using ModernizationDemo.BusinessLogic.Models;
using ModernizationDemo.BusinessLogic.Services;

namespace ModernizationDemo.SoapCore;

[ServiceContract]
public class Orders 
{
    private readonly OrderService service = new OrderService();

    [OperationContract]
    public List<OrderModel> GetOrders()
    {
        return service.GetOrders();
    }

    [OperationContract]
    public OrderModel GetOrder(int id)
    {
        return service.GetOrder(id);
    }

    [OperationContract]
    public int AddOrder(OrderCreateModel order)
    {
        return service.AddOrder(order);
    }

    [OperationContract]
    public decimal CalculateTotalPrice(OrderCreateModel order)
    {
        return service.CalculateTotalPrice(order);
    }

    [OperationContract]
    public void CompleteOrder(int id)
    {
        service.CompleteOrder(id);
    }

    [OperationContract]
    public void CancelOrder(int id)
    {
        service.CancelOrder(id);
    }
}