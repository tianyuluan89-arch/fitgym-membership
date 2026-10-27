using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FitGym.Models;
using FitGym.Services;

namespace FitGym.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Trainer")]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        /// <summary>
        /// Record check-in for a member in a class.
        /// POST: api/attendance
        /// </summary>
        [HttpPost]
        public ActionResult<Attendance> CheckIn(CheckInRequest request)
        {
            try
            {
                var attendance = _attendanceService.CheckIn(request.MemberId, request.ClassId);
                return CreatedAtAction(nameof(CheckIn), attendance);
            }
            catch (InvalidOperationException ex) when (ex.Message == "ALREADY_CHECKED_IN")
            {
                return Conflict(new { error = "Member is already checked in for this class." });
            }
        }
    }

    public class CheckInRequest
    {
        public int MemberId { get; set; }
        public int ClassId { get; set; }
    }
}
