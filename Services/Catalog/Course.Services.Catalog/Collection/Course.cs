using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Services.Catalog.Collection
{
    //MongoDb de Id tipler ObjectId şeklinde tutulduğu için class lara bu attribute ları veriyoruz
    public class Course
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Price { get; set; }

        public string UserId { get; set; }

        public string Picture { get; set; }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedTime { get; set; }

        public Feature Feature { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string CategoryId { get; set; }

        //Bu yapı MongoDb ile serialize yapıısna benze şekilde MongoDb tarafında nesnesini oluşturma göz ardı et şeklinde kullanılırç
        [BsonIgnore]
        public Category Category { get; set; }
    }
}
