namespace WebApplication.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int WalletId { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; } 
        public string Description { get; set; } 
        public Wallet Wallet { get; set; }
    }
}
