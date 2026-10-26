namespace FitGym.Models
{
    public class MembershipPlan
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int DurationMonths { get; set; }
        public decimal Price { get; set; }
        public int MaxMonthlyBookings { get; set; }

        // Navigation property
        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    }
}
