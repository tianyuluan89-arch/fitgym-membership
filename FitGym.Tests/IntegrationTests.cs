using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FitGym.Models;
using FitGym.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit;

namespace FitGym.Tests
{
    public class IntegrationTests : IClassFixture<IntegrationFixture>
    {
        private readonly IntegrationFixture _fixture;

        public IntegrationTests(IntegrationFixture fixture)
        {
            _fixture = fixture;
        }

        // POST api/attendance twice for same member+class -> 409 conflict
        [Fact]
        public async Task CheckIn_DuplicateCheckIn_ReturnsConflict()
        {
            var client = _fixture.CreateClient();
            var trainerToken = _fixture.CreateToken(1, "Trainer");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", trainerToken);

            var payload = new { memberId = 1, classId = 5 };

            var first = await client.PostAsJsonAsync("/api/attendance", payload);
            Assert.Equal(HttpStatusCode.Created, first.StatusCode);

            var second = await client.PostAsJsonAsync("/api/attendance", payload);
            Assert.Equal(HttpStatusCode.Conflict, second.StatusCode);
        }

        // Booking a class that is already full -> 409 conflict
        [Fact]
        public async Task BookClass_FullClass_ReturnsConflict()
        {
            var service = (BookingService)_fixture.GetService<IBookingService>();
            service.SeedClass(new WorkoutClass
            {
                Id = 1,
                TrainerId = 1,
                Name = "Yoga",
                Schedule = DateTime.UtcNow.AddDays(1),
                Capacity = 1,
                EnrolledCount = 1
            });
            _fixture.SeedActiveSubscription(memberId: 1);

            var client = _fixture.CreateClient();
            var memberToken = _fixture.CreateToken(1, "Member");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", memberToken);

            var response = await client.PostAsync("/api/workout-classes/1/book", null);
            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        }

        // Booking a class overlapping an existing active booking on the same schedule -> 409 conflict
        [Fact]
        public async Task BookClass_ScheduleConflict_ReturnsConflict()
        {
            var bookingService = (BookingService)_fixture.GetService<IBookingService>();
            var schedule = DateTime.UtcNow.AddDays(2);
            bookingService.SeedClass(new WorkoutClass
            {
                Id = 1,
                TrainerId = 1,
                Name = "Yoga",
                Schedule = schedule,
                Capacity = 10,
                EnrolledCount = 0
            });
            bookingService.SeedClass(new WorkoutClass
            {
                Id = 2,
                TrainerId = 1,
                Name = "Pilates",
                Schedule = schedule,
                Capacity = 10,
                EnrolledCount = 0
            });
            bookingService.SeedBooking(new Booking
            {
                Id = 1,
                MemberId = 1,
                ClassId = 1,
                BookedAt = DateTime.UtcNow,
                Status = "Booked",
                WorkoutClass = new WorkoutClass
                {
                    Id = 1,
                    TrainerId = 1,
                    Name = "Yoga",
                    Schedule = schedule,
                    Capacity = 10,
                    EnrolledCount = 0
                }
            });
            _fixture.SeedActiveSubscription(memberId: 1);

            var client = _fixture.CreateClient();
            var memberToken = _fixture.CreateToken(1, "Member");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", memberToken);

            var response = await client.PostAsync("/api/workout-classes/2/book", null);
            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        }

        // Booking without an active subscription -> 403 forbidden
        [Fact]
        public async Task BookClass_NoActiveSubscription_ReturnsForbidden()
        {
                        var subService = (SubscriptionService)_fixture.GetService<ISubscriptionService>();
            subService.ClearSubscriptions();

            var bookingService = (BookingService)_fixture.GetService<IBookingService>();
            bookingService.SeedClass(new WorkoutClass
            {
                Id = 1,
                TrainerId = 1,
                Name = "Yoga",
                Schedule = DateTime.UtcNow.AddDays(1),
                Capacity = 10,
                EnrolledCount = 0
            });

            var client = _fixture.CreateClient();
            var memberToken = _fixture.CreateToken(1, "Member");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", memberToken);

            var response = await client.PostAsync("/api/workout-classes/1/book", null);
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        // Recording a successful payment activates the linked subscription
        [Fact]
        public async Task RecordPayment_ActivatesSubscription()
        {
            var paymentService = (PaymentService)_fixture.GetService<IPaymentService>();
            paymentService.SeedSubscription(new Subscription
            {
                Id = 1,
                MemberId = 1,
                PlanId = 1,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMonths(1),
                Status = "Pending"
            });

            var client = _fixture.CreateClient();
            var memberToken = _fixture.CreateToken(1, "Member");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", memberToken);

            var payload = new { subscriptionId = 1, amount = 30m, method = "Card" };
            var response = await client.PostAsJsonAsync("/api/payments", payload);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var subscription = paymentService.GetSubscription(1);
            Assert.NotNull(subscription);
            Assert.Equal("Active", subscription!.Status);
        }
    }

    public class IntegrationFixture : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<IAttendanceService>();
                services.RemoveAll<IBookingService>();
                services.RemoveAll<ISubscriptionService>();
                services.RemoveAll<IPaymentService>();

                services.AddSingleton<IAttendanceService>(new AttendanceService());
                services.AddSingleton<IBookingService>(new BookingService());
                services.AddSingleton<ISubscriptionService>(new SubscriptionService());
                services.AddSingleton<IPaymentService>(new PaymentService());
            });
        }

        public T GetService<T>() where T : class
        {
            var scope = Services.CreateScope();
            return scope.ServiceProvider.GetRequiredService<T>();
        }

        public void SeedActiveSubscription(int memberId)
        {
            var subscriptionService = (SubscriptionService)GetService<ISubscriptionService>();
            subscriptionService.SeedSubscription(new Subscription
            {
                Id = 100,
                MemberId = memberId,
                PlanId = 1,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMonths(1),
                Status = "Active"
            });
        }

        public string CreateToken(int userId, string role)
        {
            var tokenService = new TokenService();
            return tokenService.CreateToken(userId, role);
        }
    }
}
