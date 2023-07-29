namespace Api.CoronaVirusStatistics.Domain.Entities
{
    using MongoDB.Bson.Serialization.Attributes;
    using MongoDB.Driver.GeoJsonObjectModel;
    using System;
    public interface IInfectado : IDocument
    {
        [BsonElement("nascimento")]
        DateTime DataNascimento { get; set; }
        [BsonElement("sexo")]
        String Sexo { get; set; }
        [BsonElement("localizacao")]
        GeoJson2DGeographicCoordinates Localizacao { get; set; }
    }
}
