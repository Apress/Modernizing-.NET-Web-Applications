using ModernizationDemo.SoapTests.OrdersClient;

namespace ModernizationDemo.SoapTests.Services
{
    public class SoapOrdersClient : IOrdersClient
    {
        private readonly Orders client;

        public SoapOrdersClient(string url)
        {
            this.client = new Orders() { Url = url };
        }

        public OrderModel[] GetOrders()
        {
            return client.GetOrders();
        }

        public OrderModel GetOrder(int id)
        {
            return client.GetOrder(id);
        }

        public int AddOrder(OrderCreateModel order)
        {
            return client.AddOrder(order);
        }

        public decimal CalculateTotalPrice(OrderCreateModel order)
        {
            return client.CalculateTotalPrice(order);
        }

        public void CompleteOrder(int id)
        {
            client.CompleteOrder(id);
        }

        public void CancelOrder(int id)
        {
            client.CancelOrder(id);
        }
    }
}