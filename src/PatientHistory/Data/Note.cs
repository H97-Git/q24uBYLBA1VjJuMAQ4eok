using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace PatientHistory.Data
{
    public class Note
    {
        [BsonId]
        public string Id { get; set; }

        public int PatientId { get; set; }
        public string Message { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
