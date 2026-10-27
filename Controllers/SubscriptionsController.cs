using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FitGym.Models;
using FitGym.Services;

namespace FitGym.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SubscriptionsController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionsController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        /// <summary>
        /// Create a new subscription for a member (payment handled separately).
        /// POST: api/subscriptions
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Member,Receptionist")]
        public ActionResult<Subscription> Create(CreateSubscriptionRequest request)
        {
            var subscription = _subscriptionService.CreateSubscription(request.MemberId, request.PlanId);
            return CreatedAtAction(nameof(GetActive), new { memberId = request.MemberId }, subscription);
        }

        /// <summary>
        /// Get the active subscription for a member.
        /// GET: api/subscriptions/active/{memberId}
        /// </summary>
        [HttpGet("active/{memberId}")]
        public ActionResult<Subscription> GetActive(int memberId)
        {
            var subscription = _subscriptionService.GetActiveSubscription(memberId);
            if (subscription == null)
            {
                return NotFound();
            }
            return Ok(subscription);
        }
    }

    public class CreateSubscriptionRequest
    {
        public int MemberId { get; set; }
        public int PlanId { get; set; }
    }
}
