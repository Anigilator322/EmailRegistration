using EmailRegistration.Contracts;

namespace EmailRegistration.Services
{
    public interface IVerificationService
    {
        string GenerateVerificationCode();
        bool VerifyCode(string email, string input);
    }
}