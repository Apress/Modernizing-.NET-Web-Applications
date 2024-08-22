namespace ReservationDemo.Domain.Services;

public interface ICurrentCustomerProvider
{
    Guid CustomerId { get; }
    Guid UserId { get; }
}