using FitGym.Models;

namespace FitGym.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly List<Attendance> _attendance = new();

        public Attendance CheckIn(int memberId, int classId)
        {
            // Reject duplicate check-in for the same member and class (HTTP 409 mapped by controller)
            var alreadyCheckedIn = _attendance.Any(a =>
                a.MemberId == memberId &&
                a.ClassId == classId &&
                a.Status == "Present");
            if (alreadyCheckedIn)
            {
                throw new InvalidOperationException("ALREADY_CHECKED_IN");
            }

            var attendance = new Attendance
            {
                MemberId = memberId,
                ClassId = classId,
                CheckedInAt = DateTime.UtcNow,
                Status = "Present"
            };
            _attendance.Add(attendance);
            return attendance;
        }
    }
}
