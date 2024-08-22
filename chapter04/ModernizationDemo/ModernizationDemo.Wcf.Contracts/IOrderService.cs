using System.Collections.Generic;
using System.ServiceModel;
using ModernizationDemo.BusinessLogic.Models;

namespace ModernizationDemo.Wcf.Contracts;

[ServiceContract]
public interface IOrderService
{
    [OperationContract]
    List<OrderModel> GetOrders();

    [OperationContract]
    OrderModel GetOrder(int id);

    [OperationContract]
    int AddOrder(OrderCreateModel order);

    [OperationContract]
    decimal CalculateTotalPrice(OrderCreateModel order);

    [OperationContract]
    void CompleteOrder(int id);

    [OperationContract]
    void CancelOrder(int id);
}