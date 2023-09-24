namespace TransactionsAPI.Models
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public string ApplicationName { get; set; }
        public string Email { get; set; }
        public string Filename { get; set; }
        public Uri? Url { get; set; }
        public DateTime Inception { get; set; }
        public decimal Amount { get; set; }
        public char AmountCurrency { get; set; }
        public decimal Allocation { get; set; }
    }
}
