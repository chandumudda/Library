
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace User.API.Models
{
    public class BookReview
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string BookId { get; set; }
        public string ReviewComment { get; set; }
        public string ReviewedBy { get; set; }
    }
}
