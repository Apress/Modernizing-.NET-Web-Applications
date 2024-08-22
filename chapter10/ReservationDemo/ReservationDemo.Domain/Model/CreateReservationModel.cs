using System.ComponentModel.DataAnnotations;

namespace ReservationDemo.Domain.Model;

public class CreateReservationModel : IValidatableObject
{
    [Required(ErrorMessage = "Please select the room.")]
    public int SelectedRoomId { get; set; }

    [Required(ErrorMessage = "Please select the day.")]
    public DateOnly SelectedDay { get; set; }

    [Required(ErrorMessage = "Please enter the start time.")]
    public TimeOnly StartTime { get; set; }

    [Required(ErrorMessage = "Please enter the end time.")]
    public TimeOnly EndTime { get; set; }

    public IEnumerable<ValidationResult> Validate(
        ValidationContext validationContext)
    {
        if (StartTime >= EndTime)
        {
            yield return new ValidationResult("The start of the reservation must be earlier than its end!");
        }
    }
}