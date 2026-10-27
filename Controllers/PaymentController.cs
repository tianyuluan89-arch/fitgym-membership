using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FitGym.Models;
using FitGym.Services;

namespace FitGym.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Receptionist,Member")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        /// <summary>
        /// Record a successful payment and activate the linked subscription.
        /// POST: api/payments
        /// </summary>
        [HttpPost]
        public ActionResult<Payment> Record(PaymentRequest request)
        {
            try
            {
                var payment = _paymentService.RecordPayment(request.SubscriptionId, request.Amount, request.Method);
                return CreatedAtAction(nameof(Record), payment);
            }
            catch (InvalidOperationException ex) when (ex.Message == "SUBSCRIPTION_NOT_FOUND")
            {
                return NotFound(new { error = "Subscription not found." });
            }
        }
    }

    public class PaymentRequest
    {
        public int SubscriptionId { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; } = string.Empty;
    }
}
