using Microsoft.EntityFrameworkCore;
using TransactionsAPI.Models;

namespace TransactionsAPI.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options) { }

        public DbSet<Transaction> Transactions { get; set; }
    }
}
