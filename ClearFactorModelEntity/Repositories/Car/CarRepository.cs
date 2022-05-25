using ClearFactorModelEntity.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearFactorModelEntity.Repositories
{
    public class CarRepository : Repository<Car>
    {
        public CarRepository(DbContext dbContext) : base(dbContext) { }

        public async override Task Delete(Guid id)
        {
            Car existing = await GetById(id);
            DeleteEntry(existing);
        }

        public async override Task Edit(Car incoming)
        {
            Car existing = await GetById(incoming.Id);
            UpdateEntry(existing, incoming);
        }

        public async override Task<Car> GetById(Guid id)
        {
            return await Get().FirstAsync(x => x.Id == id);
        }
    }
}
