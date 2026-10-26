namespace FitGym.Models
{
    public class Member
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime RegisteredAt { get; set; }
        public string Status { get; set; } = "Active";

        // Navigation property
        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    }
}
