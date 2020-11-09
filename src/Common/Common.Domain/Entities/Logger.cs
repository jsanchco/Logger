using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Common.Domain.Entities
{
    public class Logger
    {
        public Logger()
        {
            Timestamp = DateTime.Now;
            Lebel = Lebel.Information;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public DateTime Timestamp { get; set; }
        public Lebel Lebel { get; set; }
        public string Information { get; set; }
    }

    public enum Lebel
    {
        Debug = 1,
        Information,
        Error
    }
}
