using System;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Polly;
using ReservationDemo.Domain.Objects;
using ReservationDemo.Persistence;

namespace ReservationDemo.Domain.Repositories;

public class ReservationDayRepository(AppDbContext db)
{
    public async Task<ReservationDayDomainObject> GetDay(int roomId, DateOnly date)
    {
        var entity = await db.ReservationDays
            .Where(r => r.RoomId == roomId && r.Date == date)
            .SingleAsync();
        return MapToDomainObject(entity);
    }

    public virtual async Task<TResult> UpdateWithRetry<TResult>(
        int roomId, DateOnly date,
        Func<ReservationDayDomainObject, TResult> updateAction)
    {
        return await Policy
            .Handle<DbUpdateConcurrencyException>()
            .RetryForeverAsync()
            .ExecuteAsync(async () =>
            {
                var entity = await db.ReservationDays
                    .Where(r => r.RoomId == roomId && r.Date == date)
                    .SingleAsync();
                var day = MapToDomainObject(entity);

                var result = updateAction(day);

                MapFromDomainObject(day, entity);
                await db.SaveChangesAsync();

                return result;
            });
    }

    private ReservationDayDomainObject MapToDomainObject(
        ReservationDay entity)
    {
        var reservationObjects =
           JsonConvert.DeserializeObject<List<ReservationDomainObject>>(
               entity.ReservationsJson
           );
        return new ReservationDayDomainObject(
            entity.Id,
            entity.RoomId,
            entity.Date,
            reservationObjects);
    }

    private void MapFromDomainObject(
        ReservationDayDomainObject day, ReservationDay entity)
    {
        entity.ReservationsJson =
            JsonConvert.SerializeObject(day.Reservations);
    }
}