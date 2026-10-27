using FitGym.Models;
using FitGym.Services;
using Xunit;

namespace FitGym.Tests
{
    public class PaymentServiceTests
    {
        // Recording a payment for a subscription that does not exist throws SUBSCRIPTION_NOT_FOUND
        [Fact]
        public void RecordPayment_NonexistentSubscription_ThrowsSubscriptionNotFound()
        {
            var service = new PaymentService();
            var ex = Assert.Throws<InvalidOperationException>(() => service.RecordPayment(1, 30m, "Card"));
            Assert.Equal("SUBSCRIPTION_NOT_FOUND", ex.Message);
        }
    }
}
