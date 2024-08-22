using System;
using System.Collections.Generic;
using System.Linq;

namespace ModernizationDemo.BusinessLogic.Models;

public class OrderModel
{
    public int Id { get; set; }

    public List<OrderItemModel> OrderItems { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public DateTime? CanceledAt { get; set; }

    public decimal TotalPrice { get; set; }

    public OrderStatus Status { get; set; }

    public void CalculateProperties()
    {
        TotalPrice = OrderItems.Sum(x => x.Quantity * x.UnitPrice);
        Status = CanceledAt.HasValue ? OrderStatus.Canceled :
            CompletedAt.HasValue ? OrderStatus.Completed :
            OrderStatus.Pending;
    }
}