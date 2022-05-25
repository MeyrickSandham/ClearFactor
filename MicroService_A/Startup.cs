using ClearFactorModelEntity.Entities;
using ClearFactorModelEntity.Models;
using ClearFactorModelEntity.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Data.SqlClient;
using System.Threading;

namespace MicroService_A
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //Initialise static object environment variable context
            EnvironmentVariables.Init(false, Configuration);
            //Create/Migrate database
            MigrateDatabase();

            services.AddTransient<IUnitOfWork, UnitOfWork>(
            (unitOfWorkProvider) =>
            {
                return
                    new UnitOfWork(
                        new ApplicationContext(
                            new DbContextOptionsBuilder()
                            .UseSqlServer(EnvironmentVariables.ConnectionString)
                            .UseLazyLoadingProxies().Options
                            )
                        );
            });


            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ClearFactor", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ClearFactor v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void MigrateDatabase()
        {
            //Perform Database Migration
            using (SqlConnection conn = new SqlConnection(EnvironmentVariables.ConnectionString))
            {
                int poll = 0;
                bool connected = false;
                while (true)
                {
                    try
                    {
                        if (poll >= 30)
                        {
                            Console.WriteLine("Polling timeout - Check Database container.");
                            break;
                        }
                        conn.Open();
                        connected = true;

                        break;
                    }
                    catch (Exception ex)
                    {
                        //Connection to Database Server was made but the database doesnt exist, fresh start
                        if (ex.Message.Contains("Cannot open database"))
                        {
                            connected = true;
                            break;
                        }
                        Console.WriteLine("Waiting for Database Server to start up.");
                        poll++;
                        Thread.Sleep(500);
                    }
                }
                if (connected)
                {
                    Console.WriteLine("Connected to Database Server.");
                }
                else
                {
                    Console.WriteLine("Could not connect to Database Server.");
                    Environment.Exit(1);
                }
            }

            //Create and Migrate Database
            DbContext context =
                new ApplicationContext(
                    new DbContextOptionsBuilder()
                    .UseSqlServer(EnvironmentVariables.ConnectionString)
                    .UseLazyLoadingProxies().Options
                    );
            context.Database.Migrate();
        }
    }
}
