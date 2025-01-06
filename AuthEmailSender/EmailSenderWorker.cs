using AuthEmailSender.Services;
using EmailRegistration.Contracts;
using EmailRegistration.Settings;
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
            var connection = await factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body;
                var json = System.Text.Encoding.UTF8.GetString(body.ToArray());
                var email = System.Text.Json.JsonSerializer.Deserialize<EmailVerificationRequest>(json);
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation(" [x] Received {0}", json);
                     _sendEmailService.SendEmail(email.Email, "Email Verification", email.VerificationCode);
                }
            };
            await channel.BasicConsumeAsync(_settings.Value.EmailVerificationQueue, true, consumer);
            
        }
    }
}
