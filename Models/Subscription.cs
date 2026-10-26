namespace FitGym.Models
{
    public class Subscription
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int PlanId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = "Active";

        // Navigation properties
        public Member? Member { get; set; }
        public MembershipPlan? Plan { get; set; }
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
