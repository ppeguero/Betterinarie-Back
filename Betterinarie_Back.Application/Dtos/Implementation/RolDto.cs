using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betterinarie_Back.Application.Dtos.Implementation
{
    public class RolDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<int> UsuariosIds { get; set; } = new List<int>();
    }
}
