using System.Collections.Generic;
using System.Web.Services;
using ModernizationDemo.BusinessLogic.Models;
using ModernizationDemo.BusinessLogic.Services;

namespace ModernizationDemo.Soap
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class Orders : System.Web.Services.WebService
    {
        private readonly OrderService service = new OrderService();

        [WebMethod]
        public List<OrderModel> GetOrders()
        {
            return service.GetOrders();
        }

        [WebMethod]
        public OrderModel GetOrder(int id)
        {
            return service.GetOrder(id);
        }

        [WebMethod]
        public int AddOrder(OrderCreateModel order)
        {
            return service.AddOrder(order);
        }

        [WebMethod]
        public decimal CalculateTotalPrice(OrderCreateModel order)
        {
            return service.CalculateTotalPrice(order);
        }

        [WebMethod]
        public void CompleteOrder(int id)
        {
            service.CompleteOrder(id);
        }

        [WebMethod]
        public void CancelOrder(int id)
        {
            service.CancelOrder(id);
        }
    }
}
