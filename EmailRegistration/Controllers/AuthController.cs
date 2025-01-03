using EmailRegistration.Models;
using EmailRegistration.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmailRegistration.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private static Dictionary<string, string> _verificationCodes = new Dictionary<string, string>();
        private IVerificationService _verificationService;

        public AuthController(IVerificationService verificationService)
        {
            _verificationService = verificationService;
        }

        [HttpPost("send-code")]
        public IActionResult Index([FromBody] string email)
        {
            var code = _verificationService.GenerateVerificationCode();
            _verificationCodes[email] = code;

            var request = new EmailVerificationRequest
            {
                Email = email,
                VerificationCode = code
            };
            EmailAuthQueueProducer.SendMessage(request);

            return Ok("Verification Code sent!");
        }

        [HttpPost("verify-code")]
        public IActionResult VerifyCode([FromBody] EmailVerificationRequest request)
        {
            if (_verificationCodes.ContainsKey(request.Email) &&
                _verificationCodes[request.Email] == request.VerificationCode)
            {
                _verificationCodes.Remove(request.Email);
                return Ok("Verification successful.");
            }
            return BadRequest("Invalid code.");
        }
    }
}
