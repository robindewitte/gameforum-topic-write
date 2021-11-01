using fictivusforum_writeservice.DataModels;
using fictivusforum_writeservice.DTO;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
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
                string content = Encoding.UTF8.GetString(ea.Body.ToArray());
                if (content.StartsWith("postresponse"))
                {
                    string toConvert = content.Substring(12);
                    Response responseToPost = JsonConvert.DeserializeObject<Response>(toConvert);
                    writeController.PostResponse(responseToPost.TopicTitle, responseToPost.UserName, responseToPost.TimeOfPosting,
                        responseToPost.Content);
                }
                else if (content.StartsWith("posttopic"))
                {
                    string toConvert = content.Substring(9);
                    Topic topicToPost = JsonConvert.DeserializeObject<Topic>(toConvert);
                    writeController.PostTopic(topicToPost.Username, topicToPost.Title, topicToPost.TimeOfPosting,
                        topicToPost.Subject);
                }
                else if (content.StartsWith("deleteresponse"))
                {
                    string toConvert = content.Substring(14);
                    Response responseToDelete = JsonConvert.DeserializeObject<Response>(toConvert);
                    writeController.DeleteResponse(responseToDelete.TopicTitle, responseToDelete.UserName, responseToDelete.TimeOfPosting,
                        responseToDelete.Content);
                }
                else if (content.StartsWith("deletetopic"))
                {
                    string toConvert = content.Substring(11);
                    TopicDTO topicToDelete = JsonConvert.DeserializeObject<TopicDTO>(toConvert);
                    writeController.DeleteTopic(topicToDelete.Title);
                }
                writeController.DoWriteStuffMock();
                _channel.BasicAck(ea.DeliveryTag, false);
            };

             _channel.BasicConsume(queue: "write", autoAck: false, consumer: consumer);
            return Task.CompletedTask;
        }
    }
}
