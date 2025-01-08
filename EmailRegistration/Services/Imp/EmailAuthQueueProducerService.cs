using EmailRegistration.Infrostructure.Messages;
using EmailRegistration.Services.RabbitMq.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace EmailRegistration.Services.Imp
{
    public class EmailAuthQueueProducerService : IEmailAuthQueueProducer
    {
        private readonly string _queueName;
        private readonly string _hostName;

        public EmailAuthQueueProducerService(IOptions<RabbitMqSettings> options)
        {
            _queueName = options.Value.EmailVerificationQueue;
            _hostName = options.Value.RabbitMQUrl;
        }

        public async Task<bool> TrySendMessage(EmailVerificationMessage request)
        {
            try
            {
                var factory = new ConnectionFactory() { HostName = _hostName };
                using var connection = await factory.CreateConnectionAsync();
                using var channel = await connection.CreateChannelAsync();
                await channel.QueueDeclareAsync(
                        queue: _queueName,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                var message = JsonConvert.SerializeObject(request);
                var body = Encoding.UTF8.GetBytes(message);

                await channel.BasicPublishAsync("", _queueName, true, body);
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Failed to send message to queue");
                Console.WriteLine(ex.Message);
                return false;
            }
            
        }
    }
}

