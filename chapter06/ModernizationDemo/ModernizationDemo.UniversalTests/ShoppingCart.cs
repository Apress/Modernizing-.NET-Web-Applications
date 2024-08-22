using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernizationDemo.UniversalTests
{
    [Serializable]
    public class ShoppingCart
    {
        public DateTime Created { get; set; }
        public DateTime LastUpdated { get; set; }
        public Dictionary<string, CartItem> Items { get; set; } = new();
    }

    [Serializable]
    public class CartItem
    {
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
