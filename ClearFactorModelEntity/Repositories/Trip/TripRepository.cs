using ClearFactorModelEntity.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearFactorModelEntity.Repositories
{
    public class TripRepository : Repository<Trip>
    {
        public TripRepository(DbContext dbContext) : base(dbContext) { }

        public async override Task Delete(Guid id)
        {
            Trip existing = await GetById(id);
            DeleteEntry(existing);
        }

        public async override Task Edit(Trip incoming)
        {
            Trip existing = await GetById(incoming.Id);
            UpdateEntry(existing, incoming);
        }

        public async override Task<Trip> GetById(Guid id)
        {
            return await Get().FirstAsync(x => x.Id == id);
        }
    }
}
