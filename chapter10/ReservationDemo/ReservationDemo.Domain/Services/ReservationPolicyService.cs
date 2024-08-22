using ReservationDemo.Domain.Model;
using ReservationDemo.Domain.Repositories;
using ReservationDemo.Persistence;

namespace ReservationDemo.Domain.Services;

public class ReservationPolicyService(
    CustomerRepository customerRepository)
{
    public virtual async Task<bool> IsValidReservationLength(CreateReservationModel model, Guid customerId)
    {
        var customer = await customerRepository.Get(customerId);
        return customer.MaxReservationLength >= model.EndTime - model.StartTime;
    }
}