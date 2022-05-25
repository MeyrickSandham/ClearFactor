using ClearFactorModelEntity.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace ClearFactorModelEntity.Repositories
{
    public class DriverRepository : Repository<Driver>
    {
        public DriverRepository(DbContext dbContext) : base(dbContext) { }

        public async override Task Delete(Guid id)
        {
            Driver existing = await GetById(id);
            DeleteEntry(existing);
        }

        public async override Task Edit(Driver incoming)
        {
            Driver existing = await GetById(incoming.Id);
            UpdateEntry(existing, incoming);
        }

        public async override Task<Driver> GetById(Guid id)
        {
            return await Get().FirstAsync(x => x.Id == id);
        }
    }
}
