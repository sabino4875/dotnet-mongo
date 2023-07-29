namespace Api.CoronaVirusStatistics.Application.DTO
{
    using Newtonsoft.Json;
    using System;

    [JsonObject(MemberSerialization.OptIn)]
    public class InfectadoDto
    {
        [JsonProperty(Order = 1, PropertyName = "entityId")]
        public Nullable<Guid> EntityId { get; set; }
        [JsonProperty(Order = 2, PropertyName = "dataNascimento")]
        public Nullable<DateTime> DataNascimento { get; set; }
        [JsonProperty(Order = 3, PropertyName = "sexo")]
        public String Sexo { get; set; }
        [JsonProperty(Order = 4, PropertyName = "latitude")]
        public Nullable<Decimal> Latitude { get; set; }
        [JsonProperty(Order = 5, PropertyName = "longitude")]
        public Nullable<Decimal> Longitude { get; set; }
    }
}
