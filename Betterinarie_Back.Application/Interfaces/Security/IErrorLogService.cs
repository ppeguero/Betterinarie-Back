using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betterinarie_Back.Application.Interfaces.Security
{
    public interface IErrorLogService
    {
        Task LogErrorAsync(string message, string stackTrace, string userId);
    }
}
