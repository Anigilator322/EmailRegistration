using AuthEmailSender.Services;
using AuthEmailSender.Settings;
using EmailRegistration.Settings;

namespace AuthEmailSender
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("SmtpOptions"));
            builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMqSettings"));
            
            builder.Services.AddTransient<ISendEmailService, SendEmailService>();
            builder.Services.AddHostedService<EmailSenderWorker>();
            var host = builder.Build();
            host.Run();
        }
    }
}