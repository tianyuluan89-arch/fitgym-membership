using FitGym.Models;

namespace FitGym.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly List<Payment> _payments = new();
        private readonly List<Subscription> _subscriptions = new();

        public void SeedSubscription(Subscription s) => _subscriptions.Add(s);

        public Subscription? GetSubscription(int subscriptionId)
        {
            return _subscriptions.FirstOrDefault(s => s.Id == subscriptionId);
        }

        public Payment RecordPayment(int subscriptionId, decimal amount, string method)
        {
            var subscription = _subscriptions.FirstOrDefault(s => s.Id == subscriptionId);
            if (subscription == null)
            {
                throw new InvalidOperationException("SUBSCRIPTION_NOT_FOUND");
            }

            // Payment success activates the linked subscription
            var payment = new Payment
            {
                SubscriptionId = subscriptionId,
                Amount = amount,
                PaidAt = DateTime.UtcNow,
                Method = method,
                Status = "Success"
            };
            _payments.Add(payment);
            subscription.Status = "Active";
            return payment;
        }
    }
}
