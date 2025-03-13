using Microsoft.AspNetCore.Mvc;
using Betterinarie_Back.Core.Interfaces;
using System.Threading.Tasks;
using Betterinarie_Back.Application.Dtos.Security;
using Betterinarie_Back.Application.Interfaces.Security;

namespace Betterinarie_Back.Controllers.Security
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var (token, refreshToken) = await _authService.Login(loginDto.Email, loginDto.Password);
            if (token == null)
            {
                return Unauthorized();
            }
            return Ok(new { Token = token, RefreshToken = refreshToken });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto refreshDto)
        {
            var newToken = await _authService.RefreshToken(refreshDto.Token, refreshDto.RefreshToken);
            if (newToken == null)
            {
                return Unauthorized();
            }
            return Ok(new { Token = newToken });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutDto logoutDto)
        {
            await _authService.Logout(logoutDto.RefreshToken);
            return Ok();
        }
    }


}