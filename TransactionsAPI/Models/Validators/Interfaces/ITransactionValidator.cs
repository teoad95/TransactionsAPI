using TransactionsAPI.Models.DTO;

namespace TransactionsAPI.Models.Validators.Interfaces
{
    public interface ITransactionValidator
    {
        bool Validate(TransactionDTO? row);
    }
}
