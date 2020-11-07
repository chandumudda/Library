using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace LibraryCore.DataBaseContext
{
    public class DbRepository<T> : IDbRepository<T> where T : new()
    {
        private readonly IMongoCollection<T> _mongoDbCollection;

        public DbRepository(IMongoDbContext mongoDbContext)
        {
            var collectionName = typeof(T).Name;
            var mongoDatabase = mongoDbContext.GetDatabase();
            _mongoDbCollection = mongoDatabase.GetCollection<T>(collectionName);
        }
        public async Task<T> GetByIdAsync(Expression<Func<T, bool>> predicate)
        {
            return await _mongoDbCollection.FindSync(predicate).FirstOrDefaultAsync();
        }

        public async Task<IList<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null)
        {
            predicate ??= _ => true;
            return await _mongoDbCollection.FindSync(predicate).ToListAsync();
        }

        public async Task AddAsync(T resource)
        {
            await _mongoDbCollection.InsertOneAsync(resource);
        }

        public async Task<bool> UpdateAsync(string id, T resource)
        {
            var builder = Builders<T>.Filter;
            var filter = builder.Eq("Id", id);

            var replaceOptions = new ReplaceOptions()
            {
                IsUpsert = false
            };
            var result =
                await _mongoDbCollection.ReplaceOneAsync(filter, resource, replaceOptions);

            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(Expression<Func<T, bool>> predicate)
        {
            var result = await _mongoDbCollection.DeleteOneAsync(predicate);
            return result.DeletedCount > 0;
        }
    }
}
