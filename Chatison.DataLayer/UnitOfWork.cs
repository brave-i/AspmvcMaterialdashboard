using Chatison.Infrastructure.DataLayer;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Chatison.DataLayer
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _dataContext;
        private DbContextTransaction _dbTransaction;

        public UnitOfWork(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void BeginTransaction()
        {
            _dbTransaction = _dataContext.Database.BeginTransaction();
        }

        public int SaveChanges()
        {
            return _dataContext.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dataContext.SaveChangesAsync();
        }

        public void Commit()
        {
            _dbTransaction.Commit();
        }

        public void Rollback()
        {
            _dbTransaction.Rollback();
        }
    }
}
