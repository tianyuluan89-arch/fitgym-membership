using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FitGym.Models;

namespace FitGym.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembershipPlansController : ControllerBase
    {
        private static readonly List<MembershipPlan> _plans = new()
        {
            new MembershipPlan { Id = 1, Name = "Monthly", DurationMonths = 1, Price = 30m, MaxMonthlyBookings = 8 },
            new MembershipPlan { Id = 2, Name = "Quarterly", DurationMonths = 3, Price = 80m, MaxMonthlyBookings = 12 }
        };

        /// <summary>
        /// Get the list of membership plans. Accessible to all authenticated users.
        /// GET: api/membership-plans
        /// </summary>
        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<MembershipPlan>> GetAll()
        {
            return Ok(_plans);
        }
    }
}
