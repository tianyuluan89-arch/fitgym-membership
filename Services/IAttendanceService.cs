using FitGym.Models;

namespace FitGym.Services
{
    public interface IAttendanceService
    {
        Attendance CheckIn(int memberId, int classId);
    }
}
