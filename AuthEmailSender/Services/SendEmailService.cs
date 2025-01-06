using AuthEmailSender.Settings;
using Microsoft.Extensions.Options;
using System.Net.Mail;

namespace AuthEmailSender.Services
{
    public class SendEmailService : ISendEmailService
    {
        private readonly IOptions<SmtpOptions> _smtpOptions;
        private readonly SmtpClient _smtpClient;

        public SendEmailService(IOptions<SmtpOptions> smtpOptions)
        {
            _smtpOptions = smtpOptions;
            _smtpClient = new SmtpClient(_smtpOptions.Value.SmtpServer, _smtpOptions.Value.SmtpPort)
            {
                Credentials = new System.Net.NetworkCredential(_smtpOptions.Value.SmtpUsername, _smtpOptions.Value.SmtpPassword),
                EnableSsl = _smtpOptions.Value.EnableSsl
            };
        }

        public void SendEmail(string email, string subject, string message)
        {
            MailMessage mailMessage = new MailMessage(_smtpOptions.Value.SmtpUsername, email)
            {
                Subject = subject,
                Body = message
            };
            _smtpClient.SendAsync(mailMessage, null);
        }
    }
}
