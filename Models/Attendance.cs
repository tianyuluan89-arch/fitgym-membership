namespace FitGym.Models
{
    public class Attendance
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int ClassId { get; set; }
        public DateTime CheckedInAt { get; set; }
        public string Status { get; set; } = "Present";

        // Navigation properties
        public Member? Member { get; set; }
        public WorkoutClass? WorkoutClass { get; set; }
    }
}
