namespace TransactionsAPI.Models.DTO
{
    public class TransactionDTO
    {
        public string Id { get; set; }
        public string ApplicationName { get; set; }
        public string Email { get; set; }
        public string Filename { get; set; }
        public string Url { get; set; }
        public string Inception { get; set; }
        public string Amount { get; set; }
        public string Allocation { get; set; }
    }
}
