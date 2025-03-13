using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betterinarie_Back.Core.Entities.Implementation
{
    public class Usuario : IdentityUser<int>
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public ICollection<Consulta> Consultas { get; set; }



    }
}
