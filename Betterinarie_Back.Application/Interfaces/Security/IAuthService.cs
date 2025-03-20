using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betterinarie_Back.Application.Interfaces.Security
{
    public interface IAuthService
    {
        Task<(string Token, string RefreshToken)> Login(string email, string password);
        Task<string> RefreshToken(string token, string refreshToken);
        Task Logout(string refreshToken);
    }
}