using FitGym.Models;

namespace FitGym.Services
{
    public interface IBookingService
    {
        Booking BookClass(int memberId, int classId);
        bool CancelBooking(int bookingId);
    }
}
