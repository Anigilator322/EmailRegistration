using EmailRegistration.Settings;

namespace AuthEmailSender
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            builder.Services.AddHostedService<Worker>();
            builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMqSettings"));
            var host = builder.Build();
            host.Run();
        }
    }
}