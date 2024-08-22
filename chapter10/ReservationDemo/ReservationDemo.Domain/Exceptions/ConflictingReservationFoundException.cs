namespace ReservationDemo.Domain.Exceptions;

public class ConflictingReservationFoundException() : DomainException("The room is not available because of conflicting reservation!");