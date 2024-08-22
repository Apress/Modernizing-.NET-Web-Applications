using System.Collections.Generic;
using ModernizationDemo.BusinessLogic.Models;
using ModernizationDemo.Wcf.Contracts;

namespace ModernizationDemo.Wcf
{
    public class OrderService : IOrderService
    {
        private readonly BusinessLogic.Services.OrderService service = new BusinessLogic.Services.OrderService();

        public List<OrderModel> GetOrders()
        {
            return service.GetOrders();
        }

        public OrderModel GetOrder(int id)
        {
            return service.GetOrder(id);
        }

        public int AddOrder(OrderCreateModel order)
        {
            return service.AddOrder(order);
        }

        public decimal CalculateTotalPrice(OrderCreateModel order)
        {
            return service.CalculateTotalPrice(order);
        }

        public void CompleteOrder(int id)
        {
            service.CompleteOrder(id);
        }

        public void CancelOrder(int id)
        {
            service.CancelOrder(id);
        }

    }
}