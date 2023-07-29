namespace Api.CoronaVirusStatistics.Domain.Entities
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using System;
    public interface IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        ObjectId Id { get; set; }

        [BsonElement("entityId")]
        Guid EntityId { get; set; }
        [BsonElement("updatedAt")]
        DateTimeOffset UpdatedAt { get; }
    }
}
