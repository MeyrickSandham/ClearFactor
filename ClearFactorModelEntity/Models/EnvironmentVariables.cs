using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearFactorModelEntity.Models
{
    public static class EnvironmentVariables
    {
        public static string ConnectionString { get; set; }

        public static void Init(bool locally, IConfiguration configuration = null)
        {
            if (locally)
            {
                //Locally, running through visual studio
                //Get environmentVariables from appsettings.json
                string settings = Directory.GetCurrentDirectory() + "/../MicroService_A/appsettings.json";

                IConfigurationRoot localConfiguration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(settings)
                .Build();

                ConnectionString = BuildConnectionString(localConfiguration);
            }
            else
            {
                //Running in standalone docker container
                //OR running in docker-compose environment
                //EnvironmentVariables are passed into system by docker variable injection file
                ConnectionString = BuildConnectionString(configuration);
            }
        }

        private static string BuildConnectionString(IConfiguration configuration)
        {
            return string.Concat(new object[] {
                    "Server=",
                    configuration["DatabaseServer"],
                    ";",
                    "Database=",
                    configuration["DatabaseName"],
                    ";",
                    "User=",
                    configuration["DatabaseUser"],
                    ";",
                    "Password=",
                    configuration["DatabaseUserPassword"],
                    ";"
                });
        }
    }
}
