﻿using Betterinarie_Back.Core.Entities.Implementation.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betterinarie_Back.Application.Dtos.Implementation
{
    public class ConsultaUpdateDto
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public TimeOnly Hora { get; set; }
        public EstatusConsulta Estatus { get; set; }
        public string Motivo { get; set; }
        public int MascotaId { get; set; }
        public int VeterinarioId { get; set; }
        public List<int> MedicamentosIds { get; set; }
    }
}
