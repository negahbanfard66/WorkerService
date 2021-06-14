using Common.ObjectConvertor;
using Data.Entity.Entities;
using Data.Repository.Interfaces;
using DataConsumer_Project.Core.Middleware;
using DataConsumer_Project.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using Serilog.Formatting.Compact;
using System;
using System.Threading;
using System.Threading.Tasks;
using static DataConsumer_Project.Program;

namespace DataConsumer_Project
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IUnitOfWork _dapperUnitOfWork;
        private readonly IUnitOfWork _redisUnitOfWork;
        private readonly IConfiguration _configuration;
        readonly Serilog.ILogger _log = Log.ForContext<Worker>();

        public Worker(ILogger<Worker> logger, ServiceResolver unitOfWork, IConfiguration configuration)
        {
            _logger = logger;
            _dapperUnitOfWork = unitOfWork("DPA");
            _redisUnitOfWork = unitOfWork("RDS");
            _configuration = configuration;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogWarning($"Aplication DemoWorkerService started.");
                await StartAt(cancellationToken);
                await base.StartAsync(cancellationToken);
            }
            catch (TaskCanceledException)
            {
            }
            catch (Exception exception)
            {
                await StopAsync(cancellationToken);
                throw new CustomExceptionHandling(1, exception.Message, exception.InnerException?.Message);
            }
        }

        private async Task StartAt(CancellationToken cancellationToken)
        {
            await Task.Delay(2000, cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {

            //Log.Logger = new LoggerConfiguration()
            //.Enrich.FromLogContext()
            //.WriteTo.Console(Serilog.Events.LogEventLevel.Information, $"Project started at {DateTime.Now}.")
            //.WriteTo.File(new RenderedCompactJsonFormatter(), "/logs/log.json")
            //.WriteTo.Seq(_configuration["SeqConfig:SEQ_URL"])
            //.CreateLogger();

            //Log.Information("Project started at {@Time}.", DateTime.Now);

            _log.Information($"Project started at {DateTime.Now}");

            while (!cancellationToken.IsCancellationRequested)
            {
                StudentModel dataModel = null;

                var factory = new ConnectionFactory() { HostName = _configuration["RabbitMqSettings:Connection"], UserName = _configuration["RabbitMqSettings:Username"], Password = _configuration["RabbitMqSettings:Password"] };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "SimpleQueue",
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    var consumer = new EventingBasicConsumer(channel);

                    consumer.Received += (model, ea) =>
                    {
                        try
                        {
                            var body = ea.Body.ToArray();
                            dataModel = Serialization.FromByteArray<StudentModel>(body);

                            //Adding data to the SQL
                            _dapperUnitOfWork.Students.Add(new Student()
                            {
                                Id = dataModel.Id,
                                Name = dataModel.Name,
                                Family = dataModel.Family,
                                Tell = dataModel.Tell,
                                Address = dataModel.Address
                            });

                            _log.Information("Data inserted into SQL at {@Time}.", DateTime.Now);


                            //Adding data to the Redis
                            _redisUnitOfWork.RedisStudents.Add(new Student()
                            {
                                Id = dataModel.Id,
                                Name = dataModel.Name,
                                Family = dataModel.Family,
                                Tell = dataModel.Tell,
                                Address = dataModel.Address
                            });

                            _log.Information("Data inserted into Redis at {@Time}.", DateTime.Now);


                        }
                        catch(Exception ex)
                        {
                            throw new CustomExceptionHandling(1,ex.Message,ex.InnerException?.Message);
                        }

                    };
                    channel.BasicConsume(queue: "SimpleQueue",
                                         autoAck: true,
                                         consumer: consumer);

                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
                }

                



                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(5000, cancellationToken);
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
