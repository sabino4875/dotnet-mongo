namespace Api.CoronaVirusStatistics.Application.Services
{
    using Api.CoronaVirusStatistics.Application.DTO;
    using Api.CoronaVirusStatistics.Application.Helpers;
    using Api.CoronaVirusStatistics.Domain.Entities;
    using Api.CoronaVirusStatistics.Domain.Repositories;
    using AutoMapper;
    using MongoDB.Driver;
    using System;
    using System.Threading.Tasks;

    public class InfectadoService : IInfectadoService
    {
        private Boolean _disposable;
        private readonly IInfectadoRepository _repository;
        private readonly IMapper _mapper;

        public InfectadoService(IInfectadoRepository repository, IMapper mapper) 
        {
            _disposable = true;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task Clean()
        {
            await _repository.Clean();
        }

        public async Task<Int64> Count(FilterDefinition<Infectado> criteria)
        {
            return await _repository.Count(criteria);
        }

        public async Task<InfectadoDto> Delete(Guid entityId)
        {
            var filter = Builders<Infectado>.Filter.Eq(e => e.EntityId, entityId);
            var result = await _repository.Delete(filter);
            return _mapper.Map<InfectadoDto>(result);
        }

        protected virtual void Dispose(Boolean disposing)
        {
            if(disposing && _disposable) 
            {
                _disposable = false;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~InfectadoService()
        {
            Dispose(false);
        }

        public async Task<Boolean> Exists(FilterDefinition<Infectado> criteria)
        {
            return await _repository.Exists(criteria);
        }

        public async Task<InfectadoDto> FindOne(Guid entityId)
        {
            var filter = Builders<Infectado>.Filter.Eq(e => e.EntityId, entityId);
            var result = await _repository.FindOne(filter);
            if (result != null) 
            { 
                return _mapper.Map<InfectadoDto>(result);
            }
            return null; 
        }

        public async Task<Guid> Insert(InfectadoDto entity)
        {
            entity.EntityId = Guid.NewGuid();
            var data = _mapper.Map<Infectado>(entity);
            return await _repository.Insert(data);
        }

        public async Task<InfectadoPagedList> ListAll(InfectadoQueryParameters criteria)
        {
            return await Task.Run(() => {
                var result = new InfectadoPagedList(
                    repository: _repository,
                    mapper: _mapper,
                    criteria: criteria
                );
                return result;
            });
        }

        public async Task<InfectadoDto> Update(Guid entityId, InfectadoDto entity)
        {
            var filter = Builders<Infectado>.Filter.Eq(e => e.EntityId, entityId);
            var info = await _repository.FindOne(filter);
            var data = _mapper.Map<Infectado>(entity);
            data.Id = info.Id;
            var result = await _repository.Update(entityId, data);
            return _mapper.Map<InfectadoDto>(data);
        }
    }
}
