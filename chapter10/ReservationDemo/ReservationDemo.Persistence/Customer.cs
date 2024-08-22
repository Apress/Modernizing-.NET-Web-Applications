namespace ReservationDemo.Persistence;

public class Customer
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public TimeSpan MaxReservationLength { get; set; }
}