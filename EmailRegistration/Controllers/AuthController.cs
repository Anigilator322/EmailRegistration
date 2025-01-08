using EmailRegistration.Contracts;
using EmailRegistration.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmailRegistration.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private IAuthorizationUserService _userService;

        public AuthController(IAuthorizationUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("send-code")]
        public async Task<IActionResult> IndexAsync([FromBody] EmailVerificationRequest request)
        {
            await _userService.AuthorizeUserAsync(request.Email);

            return Ok("Verification Code sent!");
        }

        [HttpPost("verify-code")]
        public async Task<IActionResult> VerifyCode([FromBody] EmailVerificationRequest request)
        {
            var result = await _userService.VerifyCodeAsync(request.Email, request.VerificationCode);

            return Ok(new EmailVerificationResponse(request.Email, request.VerificationCode, result));
        }

        
    }
}
