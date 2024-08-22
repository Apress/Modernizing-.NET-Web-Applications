using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using ReservationDemo.Domain.Exceptions;
using ReservationDemo.Domain.Model;
using ReservationDemo.Domain.Objects;
using ReservationDemo.Domain.Repositories;
using ReservationDemo.Domain.Services;
using ReservationDemo.Persistence;

namespace ReservationDemo.Tests
{
    public class ReservationServiceTests
    {
        private readonly int testRoomId = 1;
        private readonly Guid testUserId = Guid.Empty;
        private readonly DateOnly testDay = new DateOnly(2024, 6, 23);

        private readonly ReservationDayDomainObject day;
        private readonly ReservationService reservationService;

        public ReservationServiceTests()
        {
            // prepare domain object
            day = new ReservationDayDomainObject(
                Guid.Empty, testRoomId,
                testDay,
                [
                    new ReservationDomainObject(Guid.NewGuid(), testUserId, new TimeOnly(9, 0, 0), new TimeOnly(9, 30, 0)),
                    new ReservationDomainObject(Guid.NewGuid(), testUserId, new TimeOnly(10, 0, 0), new TimeOnly(11, 30, 0)),
                    new ReservationDomainObject(Guid.NewGuid(), testUserId, new TimeOnly(13, 0, 0), new TimeOnly(16, 0, 0))
                ]
            );

            // prepare mocks
            var currentCustomerProvider = Substitute.For<ICurrentCustomerProvider>();
            currentCustomerProvider.CustomerId.Returns(Guid.Parse("897A1B7C-B7D1-44A9-A751-82AD0D0B5331"));
            currentCustomerProvider.UserId.Returns(Guid.Parse("630F65BE-A38A-4051-BFE7-385B83DA30AC"));

            var reservationPolicyService = Substitute.For<ReservationPolicyService>([null]);
            reservationPolicyService.IsValidReservationLength(Arg.Any<CreateReservationModel>(), Arg.Any<Guid>())
                .Returns(Task.FromResult(true));

            var reservationDayRepository = Substitute.For<ReservationDayRepository>([null]);
            reservationDayRepository.UpdateWithRetry(Arg.Any<int>(), Arg.Any<DateOnly>(), Arg.Any<Func<ReservationDayDomainObject, CreateReservationResult>>())
                .Returns(args =>
                {
                    var updateAction = (Func<ReservationDayDomainObject, CreateReservationResult>)args[2];
                    return updateAction(day);
                });

            reservationService = new ReservationService(reservationPolicyService, currentCustomerProvider, reservationDayRepository);
        }

        [Fact]
        public async Task MockedRepositories_SuccessfulReservation()
        {
            var result = await reservationService.CreateReservation(new CreateReservationModel()
            {
                SelectedRoomId = testRoomId,
                SelectedDay = testDay,
                StartTime = new TimeOnly(9, 30, 0),
                EndTime = new TimeOnly(10, 0, 0)
            });

            // assert the reservation was added
            Assert.Contains(day.Reservations, r => 
                r.Id == result.ReservationId 
                && r.StartTime == new TimeOnly(9, 30, 0));
        }

        [Fact]
        public async Task MockedRepositories_Conflict()
        {
            // assert the reservation was added
            await Assert.ThrowsAsync<ConflictingReservationFoundException>(() => reservationService.CreateReservation(new CreateReservationModel()
            {
                SelectedRoomId = testRoomId,
                SelectedDay = testDay,
                StartTime = new TimeOnly(9, 25, 0),
                EndTime = new TimeOnly(10, 0, 0)
            }));
        }
    }
}
