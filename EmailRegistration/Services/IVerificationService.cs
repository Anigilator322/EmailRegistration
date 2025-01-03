namespace EmailRegistration.Services
{
    public interface IVerificationService
    {
        string GenerateVerificationCode();
        bool VerifyCode(string code, string input);
    }
}