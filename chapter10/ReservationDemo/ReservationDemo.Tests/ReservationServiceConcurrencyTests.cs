using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using ReservationDemo.Domain.Exceptions;
using ReservationDemo.Domain.Model;
using ReservationDemo.Domain.Repositories;
using ReservationDemo.Domain.Services;
using ReservationDemo.Persistence;
using Xunit.Abstractions;

namespace ReservationDemo.Tests
{
    public class ReservationServiceConcurrencyTests
    {
        private readonly ITestOutputHelper testOutputHelper;

        private readonly int testRoomId = 1;
        private readonly Guid testUserId = Guid.Empty;
        private readonly DateOnly testDay = new DateOnly(2024, 1, 1);

        private readonly ServiceProvider provider;

        public ReservationServiceConcurrencyTests(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var services = new ServiceCollection();

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DB"));
            });

            services.AddScoped<ReservationPolicyService>();
            services.AddScoped<ReservationService>();

            services.AddScoped<CustomerRepository>();
            services.AddScoped<ReservationDayRepository>();

            var currentCustomerProvider = Substitute.For<ICurrentCustomerProvider>();
            currentCustomerProvider.CustomerId.Returns(Guid.Parse("897A1B7C-B7D1-44A9-A751-82AD0D0B5331"));
            currentCustomerProvider.UserId.Returns(Guid.Parse("630F65BE-A38A-4051-BFE7-385B83DA30AC"));
            services.AddSingleton<ICurrentCustomerProvider>(currentCustomerProvider);

            provider = services.BuildServiceProvider();
        }

        [Fact]
        public async Task TestConcurrentCreateReservation()
        {
            var successfulReservations = 0;
            var conflictingReservations = 0;

            using (var scope = provider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await dbContext.Database.EnsureDeletedAsync();
                await dbContext.Database.EnsureCreatedAsync();
            }

            async Task InnerJob()
            {
                using var scope = provider.CreateScope();
                var reservationService = scope.ServiceProvider.GetRequiredService<ReservationService>();

                var startTime = TimeSpan.FromMinutes(Random.Shared.Next(24 * 4) * 15);
                var endTime = startTime + TimeSpan.FromMinutes(Random.Shared.Next(8) * 15 + 15);
                if (endTime >= TimeSpan.FromHours(24))
                {
                    endTime = TimeSpan.FromHours(24) - TimeSpan.FromSeconds(1);
                }

                try
                {
                    var result = await reservationService.CreateReservation(new CreateReservationModel()
                    {
                        SelectedRoomId = testRoomId,
                        SelectedDay = testDay,
                        StartTime = TimeOnly.FromTimeSpan(startTime),
                        EndTime = TimeOnly.FromTimeSpan(endTime)
                    });

                    Interlocked.Increment(ref successfulReservations);
                }
                catch (ConflictingReservationFoundException e)
                {
                    Interlocked.Increment(ref conflictingReservations);
                }
            }

            // create 10 concurrent tasks that repeatedly create reservations
            var tasks = Enumerable.Range(0, 10)
                .Select(_ => Task.Run(async () =>
                {
                    for (int i = 0; i < 100; i++)
                    {
                        await InnerJob();
                    }
                }))
                .ToArray();

            // wait for all tasks to finish
            await Task.WhenAll(tasks);

            using (var scope = provider.CreateScope())
            {
                var reservationDayRepository = scope.ServiceProvider.GetRequiredService<ReservationDayRepository>();
                
                // load the resulting object
                var day = await reservationDayRepository.GetDay(testRoomId, testDay);
                
                // check there are no overlapping reservations
                var orderedReservations = day.Reservations
                    .OrderBy(r => r.StartTime)
                    .ToList();
                for (var i = 0; i < orderedReservations.Count - 1; i++)
                {
                    Assert.True(orderedReservations[i].EndTime <= orderedReservations[i + 1].StartTime);
                }

                Assert.Equal(successfulReservations, day.Reservations.Count);
                foreach (var reservation in orderedReservations)
                {
                    testOutputHelper.WriteLine($"{reservation.StartTime} - {reservation.EndTime}");
                }
            }
            Assert.Equal(1000, successfulReservations + conflictingReservations);
            
            testOutputHelper.WriteLine($"Successful reservations: {successfulReservations}");
            testOutputHelper.WriteLine($"Conflicting reservations: {conflictingReservations}");
        }
    }
}
