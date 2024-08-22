using Microsoft.EntityFrameworkCore;

namespace ReservationDemo.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ReservationDay> ReservationDays { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .HasData([
                    new Customer() { Id = Guid.Parse("897A1B7C-B7D1-44A9-A751-82AD0D0B5331"), Name = "Test customer", MaxReservationLength = TimeSpan.FromHours(2) }
                ]);

            modelBuilder.Entity<ReservationDay>()
                .HasData([
                    new ReservationDay()
                    {
                        Id = Guid.Parse("8B7ACBA2-8954-413E-B9DA-5D7D17C4B8E8"),
                        Date = new DateOnly(2024, 1, 1),
                        RoomId = 1,
                        ReservationsJson = "[]"
                    }
                ]);

            base.OnModelCreating(modelBuilder);
        }
    }
}
