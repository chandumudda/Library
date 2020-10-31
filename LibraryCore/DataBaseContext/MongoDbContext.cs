using System;
using LibraryCore.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LibraryCore.DataBaseContext
{
    public class MongoDbContext : IMongoDbContext
    {
        private readonly MongoClient _mongoClient;
        private readonly string _databaseName;

        public MongoDbContext(IOptions<DbConnectionDetails> connectionDetails)
        {
            if (connectionDetails?.Value.MongoConnection == null ||
                connectionDetails.Value.MongoDatabaseName == null) return;

            _mongoClient = new MongoClient(connectionDetails.Value.MongoConnection);
            _databaseName = connectionDetails.Value.MongoDatabaseName;
        }

        public IMongoDatabase GetDatabase()
        {
            try
            {
                return _mongoClient.GetDatabase(_databaseName);
            }
            catch (Exception)
            {
                throw new ArgumentNullException(nameof(_databaseName));
            }
        }
    }
}
