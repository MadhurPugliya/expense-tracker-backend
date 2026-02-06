namespace ExpenseTracker.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public ICollection<Category> Categories { get; set; }
        public ICollection<Wallet> Wallets { get; set; }
    }
}