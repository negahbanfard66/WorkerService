using Dapper.Infrastructure;
using Data.Repository.Interfaces;
using DataConsumer_Project.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Redis.Infrastructure;
using Serilog;
using System.Collections.Generic;
using System.Configuration;

namespace DataConsumer_Project
{
    public class Program
    {
        public delegate IUnitOfWork ServiceResolver(string key);
        public static void Main(string[] args)
        {
            var _config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

            CreateHostBuilder(args, _config).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args, IConfigurationRoot config) =>
            Host.CreateDefaultBuilder(args).UseSerilog().ConfigureWebHostDefaults(webBuilder =>
                    webBuilder.UseStartup<Startup>())
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                    services.AddDapperInfrastructure();
                    services.AddRedisInfrastructure(config);
                    services.AddTransient<ServiceResolver>(serviceProvider => key =>
                    {
                        switch (key)
                        {
                            case "DPA":
                                return serviceProvider.GetService<Dapper.Infrastructure.Repository.UnitOfWork>();
                            case "RDS":
                                return serviceProvider.GetService<Redis.Infrastructure.Repository.UnitOfWork>();
                            default:
                                throw new KeyNotFoundException();
                        }
                    });
                });

        public class Startup
        {
            public Startup(Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(env.ContentRootPath)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                    .AddEnvironmentVariables();

                var config = builder.Build();

                Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(config)
                    .CreateLogger();

                Log.Information("Starting up");
            }
            public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
            {
                loggerFactory.AddSerilog();
                app.UseMiddleware<SerilogMiddleware>();
            }
        }
    }
}
