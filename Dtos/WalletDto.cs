using ExpenseTracker.Models;

namespace ExpenseTracker.Dtos
{
    public class WalletCreateDto
    {
        public string Name { get; set; }
        public double Balance { get; set; }
        public WalletType Type { get; set; }
    }

    public class WalletUpdateDto
    {
        public string Name { get; set; }
        public double Balance { get; set; }
        public WalletType Type { get; set; }
    }

    public class WalletResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Balance { get; set; }
        public WalletType Type { get; set; }
        public string TypeName => Type.ToString(); // "Cash" or "Bank"
    }


    public class WalletTotalDto
    {
        public double CashTotal { get; set; }
        public double BankTotal { get; set; }
        public double GrandTotal { get; set; }
    }
}
