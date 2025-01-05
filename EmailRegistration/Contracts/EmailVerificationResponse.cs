namespace EmailRegistration.Contracts
{
    public record EmailVerificationResponse(string Email, string VerificationCode, bool IsVerified);
}
