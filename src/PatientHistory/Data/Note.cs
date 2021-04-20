using MongoDB.Bson.Serialization.Attributes;
using System;
using MongoDB.Bson;

namespace PatientHistory.Data
{
    public class Note
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public int PatientId { get; set; }
        public string Message { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
