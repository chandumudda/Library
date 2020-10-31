using LibraryCore.DataBaseContext;
using LibraryCore.TenantContext;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryCore.Extensions
{
    public static class ConfigurationSettings
    {
        public static void AddConfigurationsCommon(this IServiceCollection services)
        {
            services.AddSingleton(typeof(IMongoDbContext), typeof(MongoDbContext));
            services.AddTransient(typeof(IDbRepository<>), typeof(DbRepository<>));
            services.AddTransient(typeof(ITenantContext), typeof(TenantContext.TenantContext));
        }
    }
}
