using EmailRegistration.Contracts;

namespace EmailRegistration.Services
{
    public interface IEmailAuthQueueProducer
    {
        void SendMessage(EmailVerificationMessage request);
    }
}