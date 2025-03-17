using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betterinarie_Back.Application.Dtos.Implementation
{
    public class ConsultaDto
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Motivo { get; set; }
        public int? MascotaId { get; set; }
        public int? VeterinarioId { get; set; }
        //public List<int> MedicamentosIds { get; set; } = new List<int>();
    }
}
