using FitGym.Models;

namespace FitGym.Services
{
    public class BookingService : IBookingService
    {
        private readonly List<Booking> _bookings = new();
        private readonly List<WorkoutClass> _classes = new();

        public Booking BookClass(int memberId, int classId)
        {
            var workoutClass = _classes.FirstOrDefault(c => c.Id == classId);
            if (workoutClass == null)
            {
                throw new InvalidOperationException("CLASS_NOT_FOUND");
            }

            // Rule 1: class is full -> HTTP 409 (mapped by controller)
            if (workoutClass.EnrolledCount >= workoutClass.Capacity)
            {
                throw new InvalidOperationException("CLASS_FULL");
            }

            // Rule 2: member already has an active booking overlapping the same schedule -> HTTP 409
            var hasConflict = _bookings.Any(b =>
                b.MemberId == memberId &&
                b.Status == "Booked" &&
                b.WorkoutClass != null &&
                b.WorkoutClass.Schedule == workoutClass.Schedule);
            if (hasConflict)
            {
                throw new InvalidOperationException("SCHEDULE_CONFLICT");
            }

            // Rule 3: member subscription missing or expired -> HTTP 403 (checked via subscription service)
            // The controller verifies an active subscription before calling BookClass.

            var booking = new Booking
            {
                MemberId = memberId,
                ClassId = classId,
                BookedAt = DateTime.UtcNow,
                Status = "Booked"
            };
            _bookings.Add(booking);
            workoutClass.EnrolledCount++;
            return booking;
        }

        public bool CancelBooking(int bookingId)
        {
            var booking = _bookings.FirstOrDefault(b => b.Id == bookingId);
            if (booking == null || booking.Status != "Booked")
            {
                return false;
            }
            booking.Status = "Cancelled";
            return true;
        }
    }
}
