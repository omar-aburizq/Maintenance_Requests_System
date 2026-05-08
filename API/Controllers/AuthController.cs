using Application.Services.AuthService;
using Application.Services.AuthService.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto input)
        {
            var result = await _authService.Login(input);
            return Ok(result);
        }

        [HttpPost("ChangeUserPassword")]
        public async Task<IActionResult> ChangeUserPassword([FromBody] ChangeUserPasswordDto input)
        {
            await _authService.ChangeUserPassword(input);
            return Ok();
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto input)
        {
            var result = await _authService.RefreshToken(input);
            return Ok(result);
        }
    }
}
