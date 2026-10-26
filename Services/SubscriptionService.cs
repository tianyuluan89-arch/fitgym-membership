using FitGym.Models;

namespace FitGym.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly List<Subscription> _subscriptions = new();

        public Subscription CreateSubscription(int memberId, int planId)
        {
            // A subscription is created but not yet active until payment succeeds (402 mapped in controller)
            var subscription = new Subscription
            {
                MemberId = memberId,
                PlanId = planId,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMonths(1),
                Status = "Pending"
            };
            _subscriptions.Add(subscription);
            return subscription;
        }

        public Subscription? GetActiveSubscription(int memberId)
        {
            // Only a subscription with Status Active and a future EndDate is considered valid
            return _subscriptions.FirstOrDefault(s =>
                s.MemberId == memberId &&
                s.Status == "Active" &&
                s.EndDate > DateTime.UtcNow);
        }
    }
}
