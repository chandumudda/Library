using MongoDB.Driver;

namespace LibraryCore.DataBaseContext
{
    public interface IMongoDbContext
    {
        IMongoDatabase GetDatabase();
    }
}
