namespace Api.CoronaVirusStatistics.Application.Services
{
    using Api.CoronaVirusStatistics.Application.DTO;
    using Api.CoronaVirusStatistics.Domain.Entities;
    using MongoDB.Driver;
    using System.Threading.Tasks;
    using System;
    using Api.CoronaVirusStatistics.Application.Helpers;
    public interface IInfectadoService : IDisposable
    {
        Task<Guid> Insert(InfectadoDto entity);
        Task<InfectadoDto> Update(Guid entityId, InfectadoDto entity);
        Task<InfectadoDto> Delete(Guid entityId);
        Task<InfectadoDto> FindOne(Guid entityId);
        Task<InfectadoPagedList> ListAll(InfectadoQueryParameters criteria);
        Task Clean();
        Task<Boolean> Exists(FilterDefinition<Infectado> criteria);
        Task<Int64> Count(FilterDefinition<Infectado> criteria);
    }
}
