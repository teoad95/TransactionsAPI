using System.Linq.Expressions;
using TransactionsAPI.Models;

namespace TransactionsAPI.Repository.IRepository
{
    public interface ITransactionRepository
    {
        Task<ICollection<Transaction>> GetAllAsync();
        Task<Transaction?> GetAsync(Guid Id);
        Task CreateAsync(Transaction entity);
        Task DeleteAsync(Transaction entity);
        Task UpdateOrCreateIfNotExistAsync(Transaction entity);
    }
}
