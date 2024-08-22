using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ModernizationDemo.UniversalCoreIdentityTests
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<AppUser>(entity =>
            {
                entity.OwnsMany(u => u.ImageUrls, r => r.ToJson());
                entity.OwnsOne(u => u.ShoppingCart, r => r.ToJson().OwnsMany(c => c.Items));
            });

            base.OnModelCreating(builder);
        }
    }

    public class AppUser : IdentityUser<Guid>
    {
        public DateTime CreatedDate { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? FavoriteNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public List<AppUserImage>? ImageUrls { get; set; }
        public ShoppingCart? ShoppingCart { get; set; }
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
