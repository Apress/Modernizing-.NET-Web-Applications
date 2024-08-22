using System.Collections.Generic;

namespace ModernizationDemo.BusinessLogic.Models;

public class OrderCreateModel
{
    public List<OrderCreateItemModel> OrderItems { get; set; } = new List<OrderCreateItemModel>();

}