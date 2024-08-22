using System;
using System.Collections.Generic;
using System.Linq;
using ModernizationDemo.BusinessLogic.Exceptions;
using ModernizationDemo.BusinessLogic.Models;

namespace ModernizationDemo.BusinessLogic.Services;

// This is not a real-world implementation and is only used for demonstration purposes.
// The code is not thread-safe.
public class OrderService
{
    public List<OrderModel> GetOrders()
    {
        return Data.Orders;
    }

    public OrderModel GetOrder(int id)
    {
        return Data.Orders.SingleOrDefault(o => o.Id == id)
                      ?? throw new ItemNotFoundException();
    }

    public int AddOrder(OrderCreateModel order)
    {
        ValidateOrderCreateModel(order);

        var newOrder = new OrderModel
        {
            Id = Data.Orders.Max(o => o.Id) + 1,
            CreatedAt = DateTime.UtcNow,
            OrderItems = order.OrderItems
                .Select(i => new OrderItemModel()
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = Data.Products.Single(p => p.Id == i.ProductId).UnitPrice
                })
                .ToList()
        };
        newOrder.CalculateProperties();
        Data.Orders.Add(newOrder);

        return newOrder.Id;
    }

    public decimal CalculateTotalPrice(OrderCreateModel order)
    {
        ValidateOrderCreateModel(order);

        var newOrder = new OrderModel
        {
            OrderItems = order.OrderItems
                .Select(i => new OrderItemModel()
                {
                    Quantity = i.Quantity,
                    UnitPrice = Data.Products.Single(p => p.Id == i.ProductId).UnitPrice
                })
                .ToList()
        };
        newOrder.CalculateProperties();
        return newOrder.TotalPrice;
    }

    public void CompleteOrder(int id)
    {
        var order = GetOrder(id);
        if (order.CompletedAt != null || order.CanceledAt != null)
        {
            throw new InvalidOperationException("Order is already completed or canceled.");
        }
        order.CompletedAt = DateTime.UtcNow;
        order.Status = OrderStatus.Completed;
    }

    public void CancelOrder(int id)
    {
        var order = GetOrder(id);
        if (order.CompletedAt != null || order.CanceledAt != null)
        {
            throw new InvalidOperationException("Order is already completed or canceled.");
        }
        order.CanceledAt = DateTime.UtcNow;
        order.Status = OrderStatus.Canceled;
    }

    private static void ValidateOrderCreateModel(OrderCreateModel order)
    {
        if (order.OrderItems.Count == 0)
        {
            throw new InvalidOperationException("Order must have at least one item.");
        }
        else if (order.OrderItems.Any(o => o.Quantity <= 0))
        {
            throw new InvalidOperationException("Order item quantity must be greater than zero!");
        }
        else if (order.OrderItems.Any(o => !Data.Products.Any(p => p.IsOnSale && p.Id == o.ProductId)))
        {
            throw new InvalidOperationException("Order item product not found or not on sale any more!");
        }
    }
}