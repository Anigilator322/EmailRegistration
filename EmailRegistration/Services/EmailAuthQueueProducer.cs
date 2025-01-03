using EmailRegistration.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace EmailRegistration.Services
{
    public class EmailAuthQueueProducer
    {
        private const string _queueName = "email_verification";
        private const string _hostName = "localhost";
        public static void SendMessage(EmailVerificationRequest request)
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

