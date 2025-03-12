using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Betterinarie_Back.Core.Entities.Implementation.Enum;

namespace Betterinarie_Back.Core.Entities.Implementation
{
    public class Consulta
    {
        [Key]
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Motivo { get; set; }

        public EstatusConsulta Estatus {  get; set; }

        [ForeignKey("Mascota")]
        public int? MascotaId { get; set; }
        public Mascota Mascota { get; set; }


        [ForeignKey("Veterinario")]
        public int? VeterinarioId { get; set; }
        public Usuario Veterinario { get; set; }
        public ICollection<Medicamento> Medicamentos { get; set; } = new List<Medicamento>();
    }
}
