namespace EmailRegistration.Contracts
{
    public class EmailVerificationMessage
    {
        public string Email { get; set; } = string.Empty;
        public string VerificationCode { get; set; } = string.Empty;
    }
}
