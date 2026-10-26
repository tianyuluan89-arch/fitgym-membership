namespace FitGym.Models
{
    public class Trainer
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Specialty { get; set; }
        public DateTime HireDate { get; set; }
        public string Status { get; set; } = "Active";

        // Navigation property
        public ICollection<WorkoutClass> Classes { get; set; } = new List<WorkoutClass>();
    }
}
