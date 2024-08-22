using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ModernizationDemo.BackendApi.Data;

public class ShopDbContext(DbContextOptions<ShopDbContext> options) : IdentityDbContext(options)
{
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.OwnsMany(p => p.Prices);
        });

        base.OnModelCreating(modelBuilder);
    }
}