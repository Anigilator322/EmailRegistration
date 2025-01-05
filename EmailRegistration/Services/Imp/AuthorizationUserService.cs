using EmailRegistration.Models;
using Microsoft.AspNetCore.Identity;

namespace EmailRegistration.Services.Imp
{
    public class AuthorizationUserService : IAuthorizationUserService
    {
        private readonly UserManager<UserModel> UserManager;

        public AuthorizationUserService(UserManager<UserModel> userManager)
        {
            UserManager = userManager;
        }

        public async Task<string> AuthorizeUserAsync(string email, string verificationCode)
        {
            var user = await GetUserByEmailAsync(email);
            if (user is null)
            {
                return await CreateUserAsync(email, verificationCode);
            }
            return await UpdateVerificationCodeAsync(user, verificationCode);
        }

        public async Task<string> GetVerificationCodeByEmail(string email)
        {
            var user = await GetUserByEmailAsync(email);
            if (user is null)
            {
                Console.WriteLine("User not found");
                return string.Empty;
            }
            return user.VerificationCode;
        }

        private async Task<string> CreateUserAsync(string email, string verificationCode)
        {
            var user = new UserModel
            {
                UserName = email,
                Email = email,
                VerificationCode = verificationCode,
            };
            var result = await UserManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                Console.WriteLine("Failed to create user");
                foreach (var error in result.Errors)
                {
                    Console.WriteLine(error.Description);
                }
                return email;
            }
            Console.WriteLine("User created");
            return email;
        }

        private async Task<string> UpdateVerificationCodeAsync(UserModel user, string verificationCode)
        {
            if (user is null)
            {
                Console.WriteLine("User not found");
                return string.Empty;
            }
            user.VerificationCode = verificationCode;
            await UserManager.UpdateAsync(user);
            return user.Email;
        }

        private async Task<UserModel> GetUserByEmailAsync(string email)
        {
            var result = await UserManager.FindByEmailAsync(email);
            if (result is null)
            {
                Console.WriteLine("User not found");
                return null;
            }
            return result;
        }
    }
}
