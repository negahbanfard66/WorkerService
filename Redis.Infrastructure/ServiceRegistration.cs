using Data.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Redis.Infrastructure.Repository;

namespace Redis.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddRedisInfrastructure(this IServiceCollection services, IConfigurationRoot config)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = config.GetSection("RedisConfig").GetSection("Connection").Value + config.GetSection("RedisConfig").GetSection("Port").Value;
            });

            
            services.AddTransient<IRedisStudentRepository, RedisStudentRepository>();
            services.AddTransient<UnitOfWork>();
        }
    }
}
