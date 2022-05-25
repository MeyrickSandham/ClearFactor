using MongoDB.Bson;

namespace ClearFactorModelEntity.Models
{
    public class DriverPenaltycollectionItem
    {
        public ObjectId Id { get; set; }
        public string DriverId { get; set; }
        public int Penalty { get; set; }
    }
}
