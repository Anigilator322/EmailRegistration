namespace EmailRegistration.Services.Imp
{
    public class VerificationService : IVerificationService
    {
        public string GenerateVerificationCode()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        public bool VerifyCode(string code, string input)
        {
            return code == input;
        }
    }
}
