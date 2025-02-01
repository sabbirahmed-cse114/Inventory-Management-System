using DevSkill.Inventory.Domain;
using Microsoft.EntityFrameworkCore;

namespace DevSkill.Inventory.Infrastructure.UnitOfWorks
{
    public abstract class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbContext;
        protected ISqlUtility SqlUtility;

        public UnitOfWork(DbContext dbContext)
        {
            _dbContext = dbContext;
            SqlUtility = new SqlUtility(_dbContext.Database.GetDbConnection());
        }

        public void Dispose() => _dbContext?.Dispose();
        public ValueTask DisposeAsync() => _dbContext.DisposeAsync();
        public void Save() => _dbContext?.SaveChanges();
        public async Task SaveAsync() => await _dbContext.SaveChangesAsync();
    }
}
