using EmailRegistration.Contracts;
using Microsoft.AspNetCore.Identity;

namespace EmailRegistration.Services.Imp
{
    public class VerificationService : IVerificationService
    {
        private IAuthorizationUserService _userService;

        public VerificationService(IAuthorizationUserService userService)
        {
            _userService = userService;
        }

        public string GenerateVerificationCode()
        {
            var random = new Random();

            return random.Next(100000, 999999).ToString();
        }

        public bool VerifyCode(string email, string input)
        {
            var code = _userService.GetVerificationCodeByEmail(email).Result;
            return code == input;
        }
    }
}
