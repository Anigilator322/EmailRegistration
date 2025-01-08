using EmailRegistration.Infrostructure.Messages;
using Microsoft.AspNetCore.Identity;

namespace EmailRegistration.Services.Imp
{
    public class AuthorizationUserService : IAuthorizationUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailAuthQueueProducer _queueProducer;

        public AuthorizationUserService(UserManager<IdentityUser> userManager, IEmailAuthQueueProducer queueProducer)
        {
            _userManager = userManager;
            _queueProducer = queueProducer;
        }

        public async Task<string> AuthorizeUserAsync(string email)
        {
            if (!IsEmailValid(email))
            {
                Console.WriteLine("Invalid Email Adress Format '{0}'", email);
                return email;
            }
            var user = await GetUserByEmailAsync(email);
            if (user is null)
            {
                user = await CreateUserAsync(email);
                if(user is null)
                    return email;
            }
            await _userManager.UpdateSecurityStampAsync(user);
            var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
            SendVerificationMessage(email, token);
            return email;
        }

        public async Task<bool> VerifyCodeAsync(string email, string code)
        {
            if (!IsEmailValid(email))
            {
                Console.WriteLine("Invalid Email Adress Format '{0}'", email);
                return false;
            }
            var user = await GetUserByEmailAsync(email);
            if (user is null)
            {
                Console.WriteLine("User not found");
                return false;
            }
            var result = await _userManager.VerifyTwoFactorTokenAsync(user, "Email", code);
            return result;
        }

        private void SendVerificationMessage(string email, string token)
        {
            var message = new EmailVerificationMessage(email, token);
            _queueProducer.TrySendMessage(message);
        }

        private bool IsEmailValid(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private async Task<IdentityUser> CreateUserAsync(string email)
        {
            var user = new IdentityUser
            {
                UserName = email,
                Email = email,
            };
            try
            {
                var result = await _userManager.CreateAsync(user);
                Console.WriteLine("User created");
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to create user");
                Console.WriteLine(ex.Message);
                return user;
            }
        }

        private async Task<IdentityUser> GetUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                Console.WriteLine("User not found");
                return null;
            }
            return user;
        }
    }
}
