using EmailRegistration.Contracts;
using EmailRegistration.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace EmailRegistration.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private IVerificationService _verificationService;
        private IEmailAuthQueueProducer _queueProducer;
        private IAuthorizationUserService _userService;

        public AuthController(IVerificationService verificationService, IEmailAuthQueueProducer queueProducer, IAuthorizationUserService userService)
        {
            _verificationService = verificationService;
            _queueProducer = queueProducer;
            _userService = userService;
        }

        [HttpPost("send-code")]
        public async Task<IActionResult> IndexAsync([FromBody] EmailVerificationRequest request)
        {
            if(!IsEmailValid(request.Email))
                return BadRequest("Invalid Email");

            var code = _verificationService.GenerateVerificationCode();
            var message = new EmailVerificationMessage
            {
                Email = request.Email, 
                VerificationCode = code
            };

            await _userService.AuthorizeUserAsync(request.Email, code);
            _queueProducer.SendMessage(message);

            return Ok("Verification Code sent!");
        }

        [HttpPost("verify-code")]
        public IActionResult VerifyCode([FromBody] EmailVerificationRequest request)
        {
            var result = _verificationService.VerifyCode(request.Email, request.VerificationCode);

            return Ok(new EmailVerificationResponse(request.Email, request.VerificationCode, result));
        }

        private bool IsEmailValid(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
