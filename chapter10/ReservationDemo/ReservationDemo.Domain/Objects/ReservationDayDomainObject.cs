namespace ReservationDemo.Domain.Objects;

public class ReservationDayDomainObject
{
    private readonly List<ReservationDomainObject> reservations;

    public Guid Id { get; }
    public int RoomId { get; }
    public DateOnly Date { get; }
    public IReadOnlyList<ReservationDomainObject> Reservations
        => reservations;

    public ReservationDayDomainObject(Guid id, int roomId, DateOnly date,
        IEnumerable<ReservationDomainObject> reservations)
    {
        Id = id;
        RoomId = roomId;
        Date = date;
        this.reservations = reservations.ToList();
    }

    public bool TryAddReservation(ReservationDomainObject reservation)
    {
        if (!reservations.All(r =>
                r.EndTime <= reservation.StartTime
                || r.StartTime >= reservation.EndTime))
        {
            return false;
        }
        reservations.Add(reservation);
        return true;
    }
}