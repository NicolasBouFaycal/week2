using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Npgsql;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Common.Abstraction;
using UMS.Domain;
using UMS.Domain.LinqModels;
using UMS.Persistence;

namespace UMS.Application.HostService
{
    public class RabbitMqHost : BackgroundService
    {
        private readonly ILogger<RabbitMqHost> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConnection connection;


        public RabbitMqHost(IConnection _connection, IServiceProvider serviceProvider, ILogger<RabbitMqHost> logger)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            connection = _connection;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    /*using var channel = connection.CreateModel();
                    channel.QueueDeclare("StudentEnrollToCourses",
                         durable: true,
                         exclusive: false,
                         autoDelete: false,
                         arguments: null);
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (sender, e) =>
                    {
                        var body = e.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine(message);
                    };
                    channel.BasicConsume("StudentEnrollToCourses", true, consumer);*/
                    var message = "";
                    int i = 0;
                    var msg = "";

                        using var channel = connection.CreateModel();
                        channel.ExchangeDeclare("StudentEnrollToCourses-exchage", ExchangeType.Direct);
                        channel.QueueDeclare("StudentEnrollToCourses",
                             durable: true,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);
                        channel.QueueBind("StudentEnrollToCourses", "StudentEnrollToCourses-exchage", "account.init");

                        var consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (sender, e) =>
                        {
                            var body = e.Body.ToArray();
                             message = Encoding.UTF8.GetString(body);
                            using (var scope = _serviceProvider.CreateScope())
                            {

                                var _context = scope.ServiceProvider.GetRequiredService<MyDbContext>();
                                var _shemaService = scope.ServiceProvider.GetRequiredService<IShemaHelper>();
                                if (message != "")
                                {
                                    var cl = JsonConvert.DeserializeObject<ClassEnrollment>(message);
                                    var getStudentKeycklokId= _context.Users
                                                                .Where(u => u.Id == cl.StudentId)
                                                                .FirstOrDefault();
                                    var branch = _shemaService.getBranch(getStudentKeycklokId.KeycloakId);
                                    var conn = _context.Database.GetDbConnection() as NpgsqlConnection;
                                    _shemaService.setShema(conn, branch);
                                    ClassEnrollment classenrollment = new ClassEnrollment();
                                    classenrollment.Id = 4;
                                    classenrollment.StudentId = cl.StudentId;
                                    classenrollment.ClassId = cl.ClassId;
                                    _context.Add(classenrollment);
                                    _context.SaveChanges();
                                    Console.WriteLine("nicolas bou faycal lobee");


                                }
                            }

                        };
                        
                        channel.BasicConsume("StudentEnrollToCourses", true, consumer);
                        
                    
                        await Task.Delay(1000, stoppingToken);
                    // Wait for 1 second before processing the next message
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while processing message");
                }
            }
        }
    }
}
