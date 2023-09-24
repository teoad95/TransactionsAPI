using System.Linq.Expressions;
using TransactionsAPI.Models;

namespace TransactionsAPI.Repository.IRepository
{
    public interface ITransactionRepository
    {
        Task<List<Transaction>> GetAllAsync(int pageSize = 10, int pageNumber = 1);
        Task<Transaction?> GetAsync(Guid Id);
        Task CreateAsync(Transaction entity);
        Task DeleteAsync(Transaction entity);
        Task UpdateOrCreateIfNotExistAsync(Transaction entity);
    }
}
