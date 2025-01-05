using Microsoft.AspNetCore.Identity;

namespace EmailRegistration.Models
{
    public class UserModel : IdentityUser
    {
        public string VerificationCode { get; set; } = string.Empty;
    }
}
