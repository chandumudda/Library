
using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace User.API.Models
{
    public class BookHistory
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string BookId { get; set; }
        public string ReadyBy { get; set; }
        public DateTime ReadDate { get; set; }
    }
}
