﻿using Betterinarie_Back.Application.Dtos.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betterinarie_Back.Application.Interfaces.Implementation
{
    public interface IConsultaService
    {
        Task<ConsultaDto> GetConsultaById(int id);
        Task<IEnumerable<ConsultaDto>> GetAllConsultas();
        Task<ConsultaDto> CreateConsulta(ConsultaPostDto createDto);
        Task UpdateConsulta(ConsultaUpdateDto updateDto);
        Task DeleteConsulta(int id);
        Task AssignMedicamentoToConsulta(int consultaId, int medicamentoId);
        Task<IEnumerable<ConsultaDto>> GetConsultasByMascota(int mascotaId);
        Task<IEnumerable<ConsultaDto>> GetConsultasByVeterinario(int veterinarioId);
    }
}
