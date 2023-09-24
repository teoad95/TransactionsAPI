using TransactionsAPI.Models.DTO;

namespace TransactionsAPI.Models
{
    public interface ITransactionBuilder
    {
        Transaction Build(TransactionDTO transactionInsertData);
    }
}
