namespace FitGym.Models
{
    public class WorkoutClass
    {
        public int Id { get; set; }
        public int TrainerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime Schedule { get; set; }
        public int Capacity { get; set; }
        public int EnrolledCount { get; set; }

        // Navigation properties
        public Trainer? Trainer { get; set; }
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
