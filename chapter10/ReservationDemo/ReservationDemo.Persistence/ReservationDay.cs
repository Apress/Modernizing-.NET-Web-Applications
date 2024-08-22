using System.ComponentModel.DataAnnotations;

namespace ReservationDemo.Persistence;

public class ReservationDay
{
    public Guid Id { get; set; }

    public int RoomId { get; set; }

    public DateOnly Date { get; set; }

    public string ReservationsJson { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; }
}