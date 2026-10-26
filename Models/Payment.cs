namespace FitGym.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int SubscriptionId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaidAt { get; set; }
        public string Method { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending";

        // Navigation property
        public Subscription? Subscription { get; set; }
    }
}
