using FitGym.Models;

namespace FitGym.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly List<Subscription> _subscriptions = new();

        public void SeedSubscription(Subscription s) => _subscriptions.Add(s);
                public void ClearSubscriptions() => _subscriptions.Clear();


        public Subscription CreateSubscription(int memberId, int planId)
        {
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
            return _subscriptions.FirstOrDefault(s =>
                s.MemberId == memberId &&
                s.Status == "Active" &&
                s.EndDate > DateTime.UtcNow);
        }
    }
}
