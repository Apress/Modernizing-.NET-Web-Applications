using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using ModernizationDemo.BusinessLogic.Services;

namespace ModernizationDemo.SoapGrpc.Services
{
    public class OrdersImplementation : Orders.OrdersBase
    {

        private readonly OrderService orderService = new OrderService();

        public override async Task<GetOrdersResponse> GetOrders(GetOrdersRequest request, ServerCallContext context)
        {
            return new GetOrdersResponse()
            {
                Result = { orderService.GetOrders().Select(ToOrderModel) }
            };
        }
        public override async Task<GetOrderResponse> GetOrder(GetOrderRequest request, ServerCallContext context)
        {
            return new GetOrderResponse()
            {
                Result = ToOrderModel(orderService.GetOrder(request.Id))
            };
        }

        public override async Task<AddOrderResponse> AddOrder(AddOrderRequest request, ServerCallContext context)
        {
            return new AddOrderResponse()
            {
                Result = orderService.AddOrder(FromOrderCreateModel(request.Model))
            };
        }

        public override async Task<CalculateTotalPriceResponse> CalculateTotalPrice(CalculateTotalPriceRequest request, ServerCallContext context)
        {
            return new CalculateTotalPriceResponse()
            {
                Result = (double)orderService.CalculateTotalPrice(FromOrderCreateModel(request.Model))
            };
        }

        public override async Task<CompleteOrderResponse> CompleteOrder(CompleteOrderRequest request, ServerCallContext context)
        {
            orderService.CompleteOrder(request.Id);
            return new CompleteOrderResponse();
        }

        public override async Task<CancelOrderResponse> CancelOrder(CancelOrderRequest request, ServerCallContext context)
        {
            orderService.CancelOrder(request.Id);
            return new CancelOrderResponse();
        }

        private OrderModel ToOrderModel(BusinessLogic.Models.OrderModel o)
        {
            return new OrderModel()
            {
                Id = o.Id,
                OrderItems =
                {
                    o.OrderItems
                        .Select(i => new OrderItemModel()
                        {
                            ProductId = i.ProductId,
                            Quantity = (double)i.Quantity,
                            UnitPrice = (double)i.UnitPrice
                        })
                },
                CreatedAt = o.CreatedAt.ToTimestamp(),
                CompletedAt = o.CompletedAt?.ToTimestamp(),
                CanceledAt = o.CanceledAt?.ToTimestamp(),
                Status = o.Status.ToString(),
                TotalPrice = (double)o.TotalPrice
            };
        }

        private BusinessLogic.Models.OrderCreateModel FromOrderCreateModel(OrderCreateModel o)
        {
            return new BusinessLogic.Models.OrderCreateModel()
            {
                OrderItems = o.OrderItems
                    .Select(i => new BusinessLogic.Models.OrderCreateItemModel()
                    {
                        ProductId = i.ProductId,
                        Quantity = (decimal)i.Quantity
                    })
                    .ToList()
            };
        }
    }
}
