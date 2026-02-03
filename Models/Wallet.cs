namespace ExpenseTracker.Models
{
    public enum WalletType
    {
       Cash = 1,
       Bank = 2
    }
    public class Wallet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Balance { get; set; }
        public WalletType Type { get; set; }
    }
}