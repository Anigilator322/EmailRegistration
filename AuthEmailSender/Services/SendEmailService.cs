using AuthEmailSender.Settings;
using System.Net.Mail;

namespace AuthEmailSender.Services
{
    public class SendEmailService : ISendEmailService
    {
        private ISmtpOptions _smtpOptions;
        private SmtpClient _smtpClient;

        public SendEmailService(ISmtpOptions smtpOptions)
        {
            _smtpOptions = smtpOptions;
            _smtpClient = new SmtpClient(_smtpOptions.SmtpServer, _smtpOptions.SmtpPort)
            {
                Credentials = new System.Net.NetworkCredential(_smtpOptions.SmtpUsername, _smtpOptions.SmtpPassword),
                EnableSsl = _smtpOptions.EnableSsl
            };
        }

        public void SendEmail(string email, string subject, string message)
        {
            MailMessage mailMessage = new MailMessage(_smtpOptions.SmtpUsername, email)
            {
                Subject = subject,
                Body = message
            };
            _smtpClient.SendAsync(mailMessage, null);
        }
    }
}
