using EmailRegistration.Infrostructure.Messages;

namespace EmailRegistration.Services
{
    public interface IEmailAuthQueueProducer
    {
        public Task<bool> TrySendMessage(EmailVerificationMessage request);
    }
}