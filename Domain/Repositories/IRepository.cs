namespace Api.CoronaVirusStatistics.Domain.Repositories
{
    using Api.CoronaVirusStatistics.Domain.Entities;
    using MongoDB.Driver;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRepository<TEntity> : IDisposable where TEntity : IDocument 
    {
        Task<Guid> Insert(TEntity entity);
        Task<TEntity> Update(Guid id, TEntity entity);
        Task<TEntity> Delete(FilterDefinition<TEntity> criteria);
        Task<TEntity> FindOne(FilterDefinition<TEntity> criteria);
        Task<IEnumerable<TEntity>> ListAll(FilterDefinition<TEntity> criteria, Int32 limit, Int32 offset);
        Task Clean();
        Task<Boolean> Exists(FilterDefinition<TEntity> criteria);
        Task<Int64> Count(FilterDefinition<TEntity> criteria);
    }
}
