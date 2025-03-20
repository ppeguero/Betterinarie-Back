using Betterinarie_Back.Application.Interfaces.Security;
using Betterinarie_Back.Core.Entities.Implementation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Betterinarie_Back.Application.Services.Security
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IErrorLogService _errorLogService;

        public AuthService(UserManager<Usuario> userManager, IConfiguration configuration, IErrorLogService errorLogService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _errorLogService = errorLogService;
        }

        public async Task<(string Token, string RefreshToken)> Login(string email, string password)
        {
            try
            {
                var usuario = await _userManager.FindByEmailAsync(email);
                if (usuario == null || !await _userManager.CheckPasswordAsync(usuario, password))
                {
                    return (null, null);
                }

                var token = await GenerateJwtToken(usuario);
                var refreshToken = Guid.NewGuid().ToString();

                usuario.RefreshToken = refreshToken;
                usuario.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(30);
                await _userManager.UpdateAsync(usuario);
                return (token, refreshToken);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(
                    message: "Error al intentar iniciar sesión",
                    stackTrace: ex.StackTrace,
                    userId: null
                );
                throw;
            }
        }

        public async Task<string> RefreshToken(string token, string refreshToken)
        {
            try
            {
                var principal = GetPrincipalFromExpiredToken(token);
                var usuarioId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var usuario = await _userManager.FindByIdAsync(usuarioId);

                if (usuario == null || usuario.RefreshToken != refreshToken || usuario.RefreshTokenExpiryTime <= DateTime.UtcNow)
                {
                    return null;
                }

                var newToken = await GenerateJwtToken(usuario);
                return newToken;
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(
                    message: "Error al intentar refrescar el token",
                    stackTrace: ex.StackTrace,
                    userId: null
                );
                throw;
            }
        }

        private async Task<string> GenerateJwtToken(Usuario usuario)
        {
            var roles = await _userManager.GetRolesAsync(usuario);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email)
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var keyBytes = Convert.FromBase64String(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var keyBytes = Convert.FromBase64String(_configuration["Jwt:Key"]);
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false
            }, out _);
            return principal;
        }

        public async Task Logout(string refreshToken)
        {
            try
            {
                var usuario = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
                if (usuario != null)
                {
                    usuario.RefreshToken = null;
                    usuario.RefreshTokenExpiryTime = null;
                    await _userManager.UpdateAsync(usuario);
                }
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(
                    message: "Error al intentar cerrar sesión",
                    stackTrace: ex.StackTrace,
                    userId: null
                );
                throw;
            }
        }
    }
}