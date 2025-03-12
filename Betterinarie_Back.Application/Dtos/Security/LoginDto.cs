using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betterinarie_Back.Application.Dtos.Security
{
    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
