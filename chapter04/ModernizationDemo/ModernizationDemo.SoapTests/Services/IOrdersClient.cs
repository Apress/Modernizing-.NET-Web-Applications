using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModernizationDemo.SoapTests.OrdersClient;

namespace ModernizationDemo.SoapTests.Services
{
    public interface IOrdersClient
    {
        OrderModel[] GetOrders();

        OrderModel GetOrder(int id);

        int AddOrder(OrderCreateModel order);

        decimal CalculateTotalPrice(OrderCreateModel order);

        void CompleteOrder(int id);

        void CancelOrder(int id);
    }
}
