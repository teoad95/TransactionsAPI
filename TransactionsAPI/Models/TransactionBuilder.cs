using System.Globalization;
using TransactionsAPI.Models.DTO;

namespace TransactionsAPI.Models
{
    public class TransactionBuilder : ITransactionBuilder
    {
        public Transaction Build(TransactionDTO transactionInsertData)
        {
            var transaction = new Transaction();
            transaction.Id = Guid.Parse(transactionInsertData.Id);
            transaction.ApplicationName = transactionInsertData.ApplicationName;
            transaction.Filename = transactionInsertData.Filename;
            transaction.Email = transactionInsertData.Email;
            if (!string.IsNullOrEmpty(transactionInsertData.Url))
                transaction.Url = new Uri(transactionInsertData.Url);
            if (!string.IsNullOrEmpty(transactionInsertData.Inception))
                transaction.Inception = DateTime.ParseExact(transactionInsertData.Inception, "M/d/yyyy", CultureInfo.InvariantCulture);
            transaction.Amount = decimal.Parse(transactionInsertData.Amount.Substring(1));
            transaction.AmountCurrency = transactionInsertData.Amount[0];
            if (!string.IsNullOrEmpty(transactionInsertData.Allocation))
                transaction.Allocation = decimal.Parse(transactionInsertData.Allocation);

            return transaction;
        }
    }
}
