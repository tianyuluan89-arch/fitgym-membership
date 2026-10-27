using FitGym.Models;
using FitGym.Services;
using Xunit;

namespace FitGym.Tests
{
    public class BookingServiceTests
    {
        // Booking a class when there is no class stored throws CLASS_NOT_FOUND
        [Fact]
        public void BookClass_NoClassStored_ThrowsClassNotFound()
        {
            var service = new BookingService();
            var ex = Assert.Throws<InvalidOperationException>(() => service.BookClass(1, 999));
            Assert.Equal("CLASS_NOT_FOUND", ex.Message);
        }

        // Cancelling a booking that does not exist returns false
        [Fact]
        public void CancelBooking_NonexistentBooking_ReturnsFalse()
        {
            var service = new BookingService();
            Assert.False(service.CancelBooking(1));
        }
    }
}
