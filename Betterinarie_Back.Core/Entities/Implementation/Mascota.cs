using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Betterinarie_Back.Core.Entities.Base;

namespace Betterinarie_Back.Core.Entities.Implementation
{
    public class Mascota : EntitieBase
    {
        public string Especie { get; set; }
        public string Raza { get; set; }
        public DateTime FechaNacimiento { get; set; }

        public string? URLImagen { get; set; }
        public string? PublicIdImagen { get; set; }

        [ForeignKey("Usuario")]
        public int? UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        [ForeignKey("Cliente")]
        public int? ClienteId { get; set; }
        public Cliente Cliente { get; set; }
        public ICollection<Consulta> Consultas { get; set; } = new List<Consulta>();
    }
}
