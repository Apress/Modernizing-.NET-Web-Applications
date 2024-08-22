using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ModernizationDemo.IdentityCoreIdentityTests
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }

    public class AppUser : IdentityUser<Guid>
    {
    }

    public class AppRole : IdentityRole<Guid>
    {
    }

    public class AppUserImage
    {
        public string Url { get; set; }
    }

    public class ShoppingCart
    {
        public DateTime Created { get; set; }
        public DateTime LastUpdated { get; set; }
        public List<CartItem> Items { get; set; } = new();
    }

    public class CartItem
    {
        public string Item { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}