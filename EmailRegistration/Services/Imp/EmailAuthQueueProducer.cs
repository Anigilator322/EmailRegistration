using EmailRegistration.Contracts;
using EmailRegistration.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace EmailRegistration.Services.Imp
{
    public class EmailAuthQueueProducer : IEmailAuthQueueProducer
    {
        private readonly string _queueName;
        private readonly string _hostName;

        public EmailAuthQueueProducer(IOptions<RabbitMqSettings> options)
        {
            _queueName = options.Value.EmailVerificationQueue;
            _hostName = options.Value.RabbitMQUrl;
        }
//Make override with returned value(exception or bool)
        public void SendMessage(EmailVerificationMessage request)
        {
            var factory = new ConnectionFactory() { HostName = _hostName };
            using var connection = factory.CreateConnectionAsync().Result;
            using var channel = connection.CreateChannelAsync().Result;
            channel.QueueDeclareAsync(
                queue: _queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var message = JsonConvert.SerializeObject(request);
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublishAsync("", _queueName, true, body);
        }
    }
}

