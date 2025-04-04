﻿using Betterinarie_Back.Core.Entities.Implementation.Enum;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betterinarie_Back.Application.Dtos.Implementation
{
    public class ConsultaDto
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public TimeOnly Hora { get; set; }
        public EstatusConsulta Estatus { get; set; }
        public string Motivo { get; set; }
        public int MascotaId { get; set; }
        public int VeterinarioId { get; set; }
        public string VeterinarioNombre { get; set; }
        public List<int> MedicamentosIds { get; set; }
    }

}
