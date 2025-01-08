namespace EmailRegistration.Services
{
    public interface IAuthorizationUserService
    {
        public Task<string> AuthorizeUserAsync(string email);
        public Task<bool> VerifyCodeAsync(string email, string code);
    }
}