namespace ExpenseTracker.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        // public string Icon { get; set; } // For UI display
        public bool IsActive { get; set; } = true;
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
