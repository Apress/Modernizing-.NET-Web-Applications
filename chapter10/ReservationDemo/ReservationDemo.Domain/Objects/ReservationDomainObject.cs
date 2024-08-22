namespace ReservationDemo.Domain.Objects;

public class ReservationDomainObject
{
    public Guid Id { get; }
    public Guid UserId { get; }
    public TimeOnly StartTime { get; }
    public TimeOnly EndTime { get; }

    public ReservationDomainObject(Guid id, Guid userId, TimeOnly startTime, TimeOnly endTime)
    {
        Id = id;
        UserId = userId;
        StartTime = startTime;
        EndTime = endTime;
    }
}