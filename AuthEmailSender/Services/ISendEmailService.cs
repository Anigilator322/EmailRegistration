namespace AuthEmailSender.Services
{
    public interface ISendEmailService
    {
        void SendEmail(string email, string subject, string message);
    }
}