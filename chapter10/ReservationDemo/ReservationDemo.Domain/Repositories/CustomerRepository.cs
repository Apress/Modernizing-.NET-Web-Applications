using ReservationDemo.Persistence;

namespace ReservationDemo.Domain.Repositories;

public class CustomerRepository(AppDbContext dbContext)
{
    public async Task<Customer> Get(Guid customerId)
    {
        return await dbContext.Customers
            .FindAsync(customerId);
    }
}