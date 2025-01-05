using EmailRegistration.Models;

namespace EmailRegistration.Services
{
    public interface IAuthorizationUserService
    {
        Task<string> AuthorizeUserAsync(string email, string verificationCode);
        Task<string> GetVerificationCodeByEmail(string email);
    }
}