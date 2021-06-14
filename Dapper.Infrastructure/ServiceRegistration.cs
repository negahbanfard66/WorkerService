using Dapper.Infrastructure.Repository;
using Data.Repository.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Dapper.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddDapperInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IStudentRepository, StudentRepository>();
            services.AddTransient<UnitOfWork>();
        }
    }
}
