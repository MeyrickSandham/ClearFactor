using ClearFactorModelEntity.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace ClearFactorModelEntity.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public ApplicationContext ApplicationContext { get; set; }

        public UnitOfWork(ApplicationContext applicationContext)
        {
            ApplicationContext = applicationContext;
        }

        public DbContext GetAcquireContext()
        {
            return ApplicationContext;
        }

        public IRepository<TModel> GetRepository<TModel, TRepository>() where TModel : class
        {
            Type getType = typeof(TRepository);
            return (IRepository<TModel>)Activator.CreateInstance(Type.GetType(getType.FullName), new object[] { ApplicationContext });
        }
    }
}
