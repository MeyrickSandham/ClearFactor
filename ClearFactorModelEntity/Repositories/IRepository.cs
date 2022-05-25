using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ClearFactorModelEntity.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Get();
        abstract Task<TEntity> GetById(Guid id);
        abstract Task<IEnumerable<TEntity>> GetByPredicate(Expression<Func<TEntity, bool>> predicate);
        Task Create(TEntity incoming);
        Task CreateRange(IEnumerable<TEntity> incoming);
        abstract Task Edit(TEntity incoming);
        abstract Task Delete(Guid id);
        Task Save();
    }
}
