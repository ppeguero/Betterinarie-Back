using Betterinarie_Back.Application.Interfaces.Security;
using Betterinarie_Back.Core.Entities.Security;
using Betterinarie_Back.Infrastructure.Data;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betterinarie_Back.Application.Services.Security
{
    public class ErrorLogService : IErrorLogService
    {
        private readonly BetterinarieContext _context;

        public ErrorLogService(BetterinarieContext context)
        {
            _context = context;
        }

        public async Task LogErrorAsync(string message, string stackTrace, string userId)
        {
            var log = new LogError
            {
                Fecha = DateTime.UtcNow,
                Mensaje = message,
                StackTrace = stackTrace,
                UsuarioId = userId
            };
            await _context.LogErrors.AddAsync(log);
            await _context.SaveChangesAsync();
        }
    }
}
