using System;

namespace ClearFactorModelEntity.Models
{
    public class RabbitMQMessage
    {
        public Guid DriverId { get; set; }
        public Guid CarId { get; set; }
        public string GeoCoordinates { get; set; }
        public int Speed { get; set; }
    }
}
