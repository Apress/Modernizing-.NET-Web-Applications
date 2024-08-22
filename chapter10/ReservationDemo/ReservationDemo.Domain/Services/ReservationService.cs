using ReservationDemo.Domain.Exceptions;
using ReservationDemo.Domain.Model;
using ReservationDemo.Domain.Objects;
using ReservationDemo.Domain.Repositories;

namespace ReservationDemo.Domain.Services;

public class ReservationService(
    ReservationPolicyService reservationPolicyService,
    ICurrentCustomerProvider currentCustomerProvider,
    ReservationDayRepository reservationDayRepository
)
{
    public async Task<CreateReservationResult> CreateReservation(
        CreateReservationModel model)
    {
        if (!await reservationPolicyService.IsValidReservationLength(
                model, currentCustomerProvider.CustomerId))
        {
            throw new MaxReservationLengthExceededException();
        }

        return await reservationDayRepository.UpdateWithRetry(
            model.SelectedRoomId, model.SelectedDay,
            day =>
            {
                var reservation = new ReservationDomainObject(
                    Guid.NewGuid(),
                    currentCustomerProvider.UserId,
                    model.StartTime,
                    model.EndTime
                );
                if (!day.TryAddReservation(reservation))
                {
                    throw new ConflictingReservationFoundException();
                }
                return new CreateReservationResult(day.Id, reservation.Id);
            });     
    }
}