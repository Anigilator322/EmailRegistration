using System.ComponentModel.DataAnnotations;

namespace EmailRegistration.Models
{
    public record EmailVerificationRequest
    {
        [Required] public string Email { get; init; }
        [Required] public string VerificationCode { get; init; }
    }
}
