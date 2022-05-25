using ClearFactorModelEntity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ClearFactorModelEntity.Entities
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions options) : base(options) { }

        public DbSet<Car> CarList { get; set; }
        public DbSet<Trip> TripList { get; set; }
        public DbSet<Driver> DriverList { get; set; }
    }

    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
    {
        /// <summary>
        /// CreateDbContext
        /// </summary>
        /// <param name="args"></param>
        public ApplicationContext CreateDbContext(string[] args)
        {
            EnvironmentVariables.Init(true, null);

            DbContextOptionsBuilder<ApplicationContext> builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseSqlServer(EnvironmentVariables.ConnectionString);
            System.Console.WriteLine(EnvironmentVariables.ConnectionString);
            return new ApplicationContext(builder.Options);
        }
    }
}