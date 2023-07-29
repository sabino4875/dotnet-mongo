namespace Api.CoronaVirusStatistics.Domain.Entities
{
    using MongoDB.Bson;
    using MongoDB.Driver.GeoJsonObjectModel;
    using System;
    public class Infectado : IInfectado
    {
        public Infectado(Double latitude, Double longitude) 
        {
            Localizacao = new GeoJson2DGeographicCoordinates(longitude, latitude);
        }

        public ObjectId Id { get; set; }
        public Guid EntityId { get; set; }
        public DateTime DataNascimento { get; set; }
        public String Sexo { get; set; }
        public GeoJson2DGeographicCoordinates Localizacao { get; set; }
        public DateTimeOffset UpdatedAt => DateTime.UtcNow;
    }
}
