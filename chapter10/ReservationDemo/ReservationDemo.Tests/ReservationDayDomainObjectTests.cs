using ReservationDemo.Domain.Objects;

namespace ReservationDemo.Tests
{
    public class ReservationDayDomainObjectTests
    {
        private readonly int testRoomId = 1;
        private readonly Guid testUserId = Guid.Empty;

        [Theory]
        [InlineData("9:00", "9:30", false)]
        [InlineData("9:30", "10:00", true)]
        [InlineData("10:15", "10:30", false)]
        [InlineData("8:30", "9:30", false)]
        [InlineData("8:30", "9:00", true)]
        public void TestReservationConflict(string startTime, string endTime, bool expectedResult)
        {
            var day = new ReservationDayDomainObject(
                Guid.Empty, testRoomId,
                new DateOnly(2024, 6, 23),
                [
                    new ReservationDomainObject(Guid.NewGuid(), testUserId, new TimeOnly(9, 0, 0), new TimeOnly(9, 30, 0)),
                    new ReservationDomainObject(Guid.NewGuid(), testUserId, new TimeOnly(10, 0, 0), new TimeOnly(11, 30, 0)),
                    new ReservationDomainObject(Guid.NewGuid(), testUserId, new TimeOnly(13, 0, 0), new TimeOnly(16, 0, 0))
                ]
            );
            var newReservation = new ReservationDomainObject(Guid.NewGuid(), testUserId, TimeOnly.Parse(startTime), TimeOnly.Parse(endTime));
            var result = day.TryAddReservation(newReservation);
            Assert.Equal(expectedResult, result);
        }
    }
}