using FitGym.Models;

namespace FitGym.Services
{
    public interface ISubscriptionService
    {
        Subscription CreateSubscription(int memberId, int planId);
        Subscription? GetActiveSubscription(int memberId);
    }
}
