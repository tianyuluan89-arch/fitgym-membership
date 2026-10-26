using FitGym.Models;

namespace FitGym.Services
{
    public interface IPaymentService
    {
        Payment RecordPayment(int subscriptionId, decimal amount, string method);
    }
}
