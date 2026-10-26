namespace FitGym.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int ClassId { get; set; }
        public DateTime BookedAt { get; set; }
        public string Status { get; set; } = "Booked";

        // Navigation properties
        public Member? Member { get; set; }
        public WorkoutClass? WorkoutClass { get; set; }
    }
}
