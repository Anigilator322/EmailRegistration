namespace EmailRegistration.Infrostructure.Messages
{
    public class EmailVerificationMessage
    {
        public string Email { get; set; } = string.Empty;
        public string VerificationCode { get; set; } = string.Empty;

        public EmailVerificationMessage(string email, string verificationCode)
        {
            Email = email;
            VerificationCode = verificationCode;
        }
    }
}
