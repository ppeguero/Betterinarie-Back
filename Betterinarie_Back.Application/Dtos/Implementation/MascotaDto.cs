using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betterinarie_Back.Application.Dtos.Implementation
{
    public class MascotaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string Especie { get; set; }
        public string Raza { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public IFormFile? ImagenFile { get; set; }
        public string UrlImagen { get; set; }

        public int? ClienteId { get; set; }
        public List<int> ConsultasIds { get; set; } = new List<int>();
    }
}
