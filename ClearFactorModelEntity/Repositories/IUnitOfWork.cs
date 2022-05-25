using Microsoft.EntityFrameworkCore;

namespace ClearFactorModelEntity.Repositories
{
    public interface IUnitOfWork
    {
        DbContext GetAcquireContext();
        IRepository<T1> GetRepository<T1, T2>() where T1 : class;
    }
}
