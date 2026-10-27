using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FitGym.Models;
using FitGym.Services;

namespace FitGym.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        /// <summary>
        /// Book a class for the current member.
        /// POST: api/workout-classes/{classId}/book
        /// </summary>
        [HttpPost("api/workout-classes/{classId}/book")]
        [Authorize(Roles = "Member")]
        public ActionResult<Booking> Book(int classId)
        {
            // In this course version the member id comes from the route or a fixed value.
            int memberId = 1;
            try
            {
                var booking = _bookingService.BookClass(memberId, classId);
                return CreatedAtAction(nameof(Book), booking);
            }
            catch (InvalidOperationException ex)
            {
                return ex.Message switch
                {
                    "CLASS_FULL" or "SCHEDULE_CONFLICT" => Conflict(new { error = ex.Message }),
                    "CLASS_NOT_FOUND" => NotFound(),
                    _ => BadRequest()
                };
            }
        }

        /// <summary>
        /// Cancel a booking.
        /// DELETE: api/bookings/{id}/cancel
        /// </summary>
        [HttpDelete("{id}/cancel")]
        public ActionResult Cancel(int id)
        {
            var cancelled = _bookingService.CancelBooking(id);
            if (!cancelled)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
