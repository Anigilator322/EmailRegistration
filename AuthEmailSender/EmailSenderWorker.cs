using AuthEmailSender.Services;
using EmailRegistration.Infrostructure.Messages;
using EmailRegistration.Services.RabbitMq.Settings;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AuthEmailSender
{
    public class EmailSenderWorker : BackgroundService
    {
        private readonly ILogger<EmailSenderWorker> _logger;
        private readonly IOptions<RabbitMqSettings> _settings;
        private readonly ISendEmailService _sendEmailService;

        public EmailSenderWorker(ILogger<EmailSenderWorker> logger, IOptions<RabbitMqSettings> settings, ISendEmailService sendEmailService)
        {
            _logger = logger;
            _settings = settings;
            _sendEmailService = sendEmailService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory() { HostName = _settings.Value.RabbitMQUrl };
            IConnection connection = null;
            IChannel channel = null;

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    connection = await factory.CreateConnectionAsync();
                    channel = await connection.CreateChannelAsync();
                    await channel.QueueDeclarePassiveAsync(_settings.Value.EmailVerificationQueue);
                    break;
                }
                catch (RabbitMQ.Client.Exceptions.OperationInterruptedException ex)
                {
                    _logger.LogWarning("Queue '{QueueName}' not created. Awaiting...", _settings.Value.EmailVerificationQueue);
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error with connection to RabbitMQ");
                    await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
                }
            }

            if (stoppingToken.IsCancellationRequested)
                return;

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body;
                var json = System.Text.Encoding.UTF8.GetString(body.ToArray());
                var email = System.Text.Json.JsonSerializer.Deserialize<EmailVerificationMessage>(json);

                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation(" [x] Received {0} {1}", json, DateTime.Now);
                    _sendEmailService.SendEmail(email.Email, "Email Verification", email.VerificationCode);
                }
            };

            await channel.BasicConsumeAsync(queue: _settings.Value.EmailVerificationQueue, autoAck: true, consumer: consumer);

            _logger.LogInformation("Listening on: {QueueName}", _settings.Value.EmailVerificationQueue);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }

}
