using System;
using System.Linq;
using Grpc.Net.Client;
using ModernizationDemo.SoapGrpc;

namespace ModernizationDemo.SoapTests.Services
{
    public class GrpcOrdersClient : IOrdersClient
    {
        private readonly Orders.OrdersClient client;

        public GrpcOrdersClient(string url)
        {
            var channel = GrpcChannel.ForAddress(url);
            client = new Orders.OrdersClient(channel);
        }

        public OrdersClient.OrderModel[] GetOrders()
        {
            return client.GetOrders(new GetOrdersRequest()).Result
                .Select(ToOrderModel)
                .ToArray();
        }

        public OrdersClient.OrderModel GetOrder(int id)
        {
            return ToOrderModel(client.GetOrder(new GetOrderRequest() { Id = id }).Result);
        }

        public int AddOrder(OrdersClient.OrderCreateModel order)
        {
            return client.AddOrder(new AddOrderRequest() { Model = FromOrderCreateModel(order) }).Result;
        }

        public decimal CalculateTotalPrice(OrdersClient.OrderCreateModel order)
        {
            return (decimal)client.CalculateTotalPrice(new CalculateTotalPriceRequest() { Model = FromOrderCreateModel(order) }).Result;
        }

        public void CompleteOrder(int id)
        {
            client.CompleteOrder(new CompleteOrderRequest() { Id = id });
        }

        public void CancelOrder(int id)
        {
            client.CancelOrder(new CancelOrderRequest() { Id = id });
        }

        private OrdersClient.OrderModel ToOrderModel(SoapGrpc.OrderModel o)
        {
            return new OrdersClient.OrderModel
            {
                Id = o.Id,
                OrderItems = o.OrderItems
                    .Select(i => new OrdersClient.OrderItemModel()
                    {
                        ProductId = i.ProductId,
                        Quantity = (decimal)i.Quantity,
                        UnitPrice = (decimal)i.UnitPrice + 0.00m
                    }).ToArray(),
                CreatedAt = o.CreatedAt.ToDateTime(),
                CompletedAt = o.CompletedAt?.ToDateTime(),
                CanceledAt = o.CanceledAt?.ToDateTime(),
                Status = (OrdersClient.OrderStatus)Enum.Parse(typeof(OrdersClient.OrderStatus), o.Status),
                TotalPrice = (decimal)o.TotalPrice + 0.00m
            };
        }

        private SoapGrpc.OrderCreateModel FromOrderCreateModel(OrdersClient.OrderCreateModel order)
        {
            return new SoapGrpc.OrderCreateModel
            {
                OrderItems = { 
                    order.OrderItems
                        .Select(i => new SoapGrpc.OrderCreateItemModel()
                        {
                            ProductId = i.ProductId,
                            Quantity = (double)i.Quantity
                        })
                }
            };
        }
    }
}