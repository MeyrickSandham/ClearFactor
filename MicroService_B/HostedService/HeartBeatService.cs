using ClearFactorModelEntity.Entities;
using ClearFactorModelEntity.Models;
using ClearFactorModelEntity.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using MyTime = System.Timers;

namespace MicroService_B.HostedService
{
    public class HeartBeatService : BackgroundService
    {
        private MyTime.Timer Timer { get; set; }

        public HeartBeatService()
        {
            Timer = new MyTime.Timer(10000)
            {
                AutoReset = true
            };
            Timer.Elapsed += PerformWork;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Timer.Enabled = true;
            Timer.Start();

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(5000, stoppingToken);
            }

            Timer.Enabled = false;
            Timer.Stop();
            Timer.Dispose();
        }

        private async void PerformWork(object sender, ElapsedEventArgs e)
        {
            DbContext context =
                new ApplicationContext(
                    new DbContextOptionsBuilder()
                    .UseSqlServer(EnvironmentVariables.ConnectionString)
                    .UseLazyLoadingProxies().Options);

            IUnitOfWork unitOfWork = new UnitOfWork(context as ApplicationContext);

            IRepository<Trip> tripRepository = unitOfWork.GetRepository<Trip, TripRepository>();
            IRepository<Driver> driverRepository = unitOfWork.GetRepository<Driver, DriverRepository>();
            IRepository<Car> carRepository = unitOfWork.GetRepository<Car, CarRepository>();

            if (await tripRepository.Get().AnyAsync())
            {
                List<Trip> activeTrips = await tripRepository.Get().ToListAsync();

                foreach (Trip trip in activeTrips)
                {
                    Car car = await carRepository.GetById(trip.CarId);
                    Driver driver = await driverRepository.GetById(car.DriverId);

                    if (trip.Active)
                    {
                        if (trip.HeartBeatCount >= 5)
                        {
                            trip.Active = false;
                            trip.EndLocation = "111.11111:111.11111";                            

                            await tripRepository.Edit(trip);
                            await tripRepository.Save();
                        }

                        Random random = new Random();
                        
                        RabbitMQMessage message = new RabbitMQMessage
                        {
                            CarId = trip.CarId,
                            DriverId = driver.Id,
                            GeoCoordinates = "000.00000:000.00000",
                            Speed = random.Next(0, 120)
                        };

                        SendQueueMessage(message);

                        //Logic to end a trip and stop heartbeats from beng generated
                        trip.HeartBeatCount++;

                        await tripRepository.Edit(trip);
                        await tripRepository.Save();
                    }
                }
            }
        }

        private void SendQueueMessage(RabbitMQMessage message)
        {
            ConnectionFactory factory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            using (IConnection connection = factory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: "letterbox",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    string jsonEncoded = JsonConvert.SerializeObject(message);

                    byte[] encoded = Encoding.UTF8.GetBytes(jsonEncoded);

                    channel.BasicPublish("", "letterbox", null, encoded);
                }
            }
        }
    }
}
