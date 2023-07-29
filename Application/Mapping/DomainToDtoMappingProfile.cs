namespace Api.CoronaVirusStatistics.Application.Mapping
{
    using Api.CoronaVirusStatistics.Application.DTO;
    using Api.CoronaVirusStatistics.Domain.Entities;
    using AutoMapper;
    using System;
    public class DomainToDtoMappingProfile : Profile
    {
        public DomainToDtoMappingProfile() 
        {
            CreateMap<Infectado, InfectadoDto>()
                .ForMember(d => d.Latitude, opt => opt.MapFrom(src => src.Localizacao.Latitude))
                .ForMember(d => d.Longitude, opt => opt.MapFrom(src => src.Localizacao.Longitude))
                .ReverseMap()
                .ForCtorParam("latitude", opt => opt.MapFrom(src => src.Latitude))
                .ForCtorParam("longitude", opt => opt.MapFrom(src => src.Longitude));
        }
    }
}
