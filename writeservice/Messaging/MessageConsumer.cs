using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using writeservice.Controllers;

namespace writeservice.Messaging
{
    public class MessageConsumer: BackgroundService
    {
        private ConnectionFactory _factory;
        private IConnection _connection;
        private IModel _channel;

        public MessageConsumer()
        {
            _factory = new ConnectionFactory() { HostName = "localhost" };

            _connection = _factory.CreateConnection();

            _channel = _connection.CreateModel();

            _channel.QueueDeclare(
                queue: "write",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            WriteController writeController = new WriteController();

            if (stoppingToken.IsCancellationRequested)
            {
                _channel.Dispose();
                _connection.Dispose();
                return Task.CompletedTask;
            }

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (model, ea) =>
            {
                writeController.DoWriteStuffMock();
                _channel.BasicAck(ea.DeliveryTag, false);
            };

             _channel.BasicConsume(queue: "write", autoAck: false, consumer: consumer);
            return Task.CompletedTask;
        }
    }
}
