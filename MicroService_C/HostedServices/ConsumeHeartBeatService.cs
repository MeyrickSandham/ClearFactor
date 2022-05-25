using ClearFactorModelEntity.Models;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MicroService_C.HostedServices
{
    public class ConsumeHeartBeatService : BackgroundService
    {
        private ConnectionFactory Factory { get; set; }
        private IConnection Connection { get; set; }
        private IModel Channel { get; set; }
        private EventingBasicConsumer Consumer { get; set; }

        private MongoClient MongoClient { get; set; }

        public ConsumeHeartBeatService()
        {
            MongoClient = new MongoClient("mongodb://127.0.0.1:27017");

            Factory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            Connection = Factory.CreateConnection();
            Channel = Connection.CreateModel();

            Channel.QueueDeclare(
                queue: "letterbox",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            Consumer = new EventingBasicConsumer(Channel);

            Consumer.Received += ConsumeMessage;

            Channel.BasicConsume(queue: "letterbox", autoAck: true, Consumer);
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(5000, stoppingToken);
            }
        }

        private void ConsumeMessage(object sender, BasicDeliverEventArgs eventArgs)
        {
            try
            {
                byte[] body = eventArgs.Body.ToArray();
                string jsonMessage = Encoding.UTF8.GetString(body);

                RabbitMQMessage message = JsonConvert.DeserializeObject<RabbitMQMessage>(jsonMessage);

                CheckSpeedLimit(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void CheckSpeedLimit(RabbitMQMessage message)
        {
            int sixtyLimitPointPenalty = 0;
            int eightyLimitPointPenalty = 0;
            int hundredLimitPointPenalty = 0;

            int pointPenalty = 0;

            if (message.Speed > 60)
            {
                sixtyLimitPointPenalty = message.Speed - 60;

                pointPenalty = sixtyLimitPointPenalty;
            }

            if (message.Speed > 80)
            {
                eightyLimitPointPenalty += message.Speed - 80;

                pointPenalty += eightyLimitPointPenalty * 2;
            }

            if (message.Speed > 100)
            {
                hundredLimitPointPenalty += message.Speed - 100;

                pointPenalty += hundredLimitPointPenalty * 5;
            }            

            if (pointPenalty > 0)
            {
                StorePenalty(message.DriverId, pointPenalty);
            }
        }

        private void StorePenalty(Guid driverId, int penaltyPoint)
        {
            IMongoDatabase db = MongoClient.GetDatabase("ClearFactor");

            IMongoCollection<BsonDocument> driverPenalty = db.GetCollection<BsonDocument>("DriverPenalty");

            BsonDocument doc = new BsonDocument
                {
                    {"DriverId", driverId.ToString() },
                    {"Penalty", penaltyPoint}
                };

            driverPenalty.InsertOne(doc);
        }
    }
}