namespace Api.CoronaVirusStatistics.InfraStructure.Repositories
{
    using Api.CoronaVirusStatistics.Domain.Entities;
    using Api.CoronaVirusStatistics.Domain.Repositories;
    using Api.CoronaVirusStatistics.InfraStructure.Context;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : IDocument
    {
        private Boolean _disposable;
        private readonly IMongoCollection<TEntity> _collection;

        public Repository(ApplicationDBContext context, String collectionName)
        {
            _disposable = true;
            _collection = context.Database.GetCollection<TEntity>(collectionName);
        }

        public async Task<TEntity> Delete(FilterDefinition<TEntity> criteria)
        {
            var result = await _collection.FindOneAndDeleteAsync(criteria);
            return result;
        }

        protected virtual void Dispose(Boolean disposing)
        {
            if (_disposable && disposing)
            {
                _disposable = false;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Repository()
        {
            Dispose(false);
        }

        public async Task<TEntity> FindOne(FilterDefinition<TEntity> criteria)
        {
            var result =  await _collection.FindAsync(criteria);
            return result.FirstOrDefault();
        }

        public async Task<Guid> Insert(TEntity entity)
        {
            await _collection.InsertOneAsync(entity);
            return entity.EntityId;
        }

        public async Task<IEnumerable<TEntity>> ListAll(FilterDefinition<TEntity> criteria, Int32 limit, Int32 offset)
        {
            return await _collection.Find(criteria).Skip(offset).Limit(limit).ToListAsync();
        }

        public async Task<TEntity> Update(Guid id, TEntity entity)
        {
            var filter = Builders<TEntity>.Filter.Eq(doc => doc.EntityId, id);
            var result = await _collection.FindOneAndReplaceAsync(filter, entity);
            return result;
        }

        public async Task Clean()
        {
            await _collection.DeleteManyAsync(new BsonDocument());
        }

        public async Task<Boolean> Exists(FilterDefinition<TEntity> criteria)
        {
            var result = await _collection.CountDocumentsAsync(criteria);
            return result > 0;
        }

        public async Task<Int64> Count(FilterDefinition<TEntity> criteria)
        {
            return await _collection.CountDocumentsAsync(criteria);
        }
    }
}
