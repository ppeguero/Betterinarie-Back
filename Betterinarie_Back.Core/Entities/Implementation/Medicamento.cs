﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betterinarie_Back.Core.Entities.Implementation
{
    public class Medicamento
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Dosis { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public int Stock { get; set; }
        public DateTime FechaCreacion { get; set; }
        public ICollection<Consulta> Consultas { get; set; } = new List<Consulta>();
    }
}
