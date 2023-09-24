using Microsoft.EntityFrameworkCore;
using TransactionsAPI.Data;
using TransactionsAPI.Models;
using TransactionsAPI.Repository.IRepository;

namespace TransactionsAPI.Repository
{
    public class TransactionsRepository : ITransactionRepository
    {
        private readonly ApplicationDBContext _db;

        public TransactionsRepository(ApplicationDBContext db)
        {
            _db = db;
        }

        public async Task<ICollection<Transaction>> GetAllAsync()
        {
            return await _db.Transactions.ToListAsync();
        }

        public async Task<Transaction?> GetAsync(Guid Id)
        {
            return await _db.Transactions.FirstOrDefaultAsync(x=> x.Id == Id);
        }

        public async Task CreateAsync(Transaction entity)
        {
            await _db.Transactions.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Transaction entity)
        {
            var transaction = await GetAsync(entity.Id);
            _db.Transactions.Remove(transaction);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateOrCreateIfNotExistAsync(Transaction entity)
        {
            var transaction = await GetAsync(entity.Id);
            if (transaction == null)
            {
                await CreateAsync(entity);
                return;
            }

            if (transaction.AmountCurrency != entity.AmountCurrency)
                throw new ArgumentException(
                    $"Transaction with id = {entity.Id}. Changing the currency of an existing transaction is not allowed.");

            transaction.ApplicationName = entity.ApplicationName;
            transaction.Email = entity.Email;
            transaction.Filename = entity.Filename;
            transaction.Url = entity.Url;
            transaction.Inception = entity.Inception;
            transaction.Amount = entity.Amount;
            transaction.Allocation = entity.Allocation;
            await _db.SaveChangesAsync();
        }
    }
}
