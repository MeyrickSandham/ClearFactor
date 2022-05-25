using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ClearFactorModelEntity.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private DbContext CurrentContext { get; set; }

        protected Repository(DbContext dbContext)
        {
            CurrentContext = dbContext;
        }
        public IQueryable<TEntity> Get()
        {
            return CurrentContext.Set<TEntity>();
        }

        public abstract Task<TEntity> GetById(Guid id);

        public async Task<IEnumerable<TEntity>> GetByPredicate(Expression<Func<TEntity, bool>> predicate)
        {
            return await Get().Where(predicate).ToListAsync();
        }

        public async Task Create(TEntity incoming)
        {
            await CurrentContext.AddAsync(incoming);
        }

        public async Task CreateRange(IEnumerable<TEntity> incoming)
        {
            await CurrentContext.AddRangeAsync(incoming);
        }

        public abstract Task Edit(TEntity incoming);

        public abstract Task Delete(Guid id);

        public async Task Save()
        {
            await CurrentContext.SaveChangesAsync();
        }       

        protected void UpdateEntry<T>(T existing, T incoming) where T : class
        {
            CurrentContext.Entry(existing).CurrentValues.SetValues(incoming);
        }

        protected void DeleteEntry<T>(T incoming) where T : class
        {
            CurrentContext.Entry(incoming).State = EntityState.Deleted;
        }        
    }
}
