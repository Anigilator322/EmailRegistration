using System.ComponentModel.DataAnnotations;

namespace EmailRegistration.Contracts
{
    public class EmailVerificationRequest
    {
        [Required] public string Email { get; set; } = string.Empty;
        public string VerificationCode { get; set; } = string.Empty;
    }
}
