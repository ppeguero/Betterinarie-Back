using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Betterinarie_Back.Application.Dtos.Implementation;
using Betterinarie_Back.Application.Interfaces.Implementation;
using Betterinarie_Back.Application.Interfaces.Security;
using Betterinarie_Back.Core.Entities.Implementation;
using Betterinarie_Back.Core.Interfaces.Implementation;
using Betterinarie_Back.Core.Entities.Implementation.Enum;

namespace Betterinarie_Back.Application.Services.Implementation
{
    public class ConsultaService : IConsultaService
    {
        private readonly IConsultaRepository _consultaRepository;
        private readonly IRepository<Medicamento> _medicamentoRepository;
        private readonly IMapper _mapper;
        private readonly IErrorLogService _errorLogService;

        public ConsultaService(IConsultaRepository consultaRepository, IRepository<Medicamento> medicamentoRepository, IMapper mapper, IErrorLogService errorLogService)
        {
            _consultaRepository = consultaRepository;
            _medicamentoRepository = medicamentoRepository;
            _mapper = mapper;
            _errorLogService = errorLogService;
        }

        public async Task<ConsultaDto> GetConsultaById(int id)
        {
            try
            {
                var consulta = await _consultaRepository.GetById(id, c => c.Medicamentos);
                if (consulta == null) return null;
                return _mapper.Map<ConsultaDto>(consulta);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(
                    message: "Error al obtener consulta por ID",
                    stackTrace: ex.StackTrace,
                    userId: null
                );
                throw;
            }
        }

        public async Task<IEnumerable<ConsultaDto>> GetAllConsultas()
        {
            try
            {
                var consultas = await _consultaRepository.GetAll(c => c.Medicamentos);
                return _mapper.Map<IEnumerable<ConsultaDto>>(consultas);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(
                    message: "Error al obtener todas las consultas",
                    stackTrace: ex.StackTrace,
                    userId: null
                );
                throw;
            }
        }

        public async Task<ConsultaDto> CreateConsulta(ConsultaDto createDto)
        {
            try
            {
                var consulta = _mapper.Map<Consulta>(createDto);
                await _consultaRepository.Add(consulta);
                return _mapper.Map<ConsultaDto>(consulta);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(
                    message: "Error al crear consulta",
                    stackTrace: ex.StackTrace,
                    userId: null
                );
                throw;
            }
        }

        public async Task UpdateConsulta(ConsultaDto updateDto)
        {
            try
            {
                var consulta = await _consultaRepository.GetById(updateDto.Id);
                if (consulta == null) throw new Exception("Consulta no encontrada");

                if (!EsCambioDeEstadoValido(consulta.Estatus, updateDto.Estatus))
                {
                    throw new Exception("Cambio de estado no permitido");
                }

                _mapper.Map(updateDto, consulta);
                await _consultaRepository.Update(consulta);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(
                    message: "Error al actualizar consulta",
                    stackTrace: ex.StackTrace,
                    userId: null
                );
                throw;
            }
        }

        private bool EsCambioDeEstadoValido(EstatusConsulta estadoActual, EstatusConsulta nuevoEstado)
        {
            var transicionesValidas = new Dictionary<EstatusConsulta, List<EstatusConsulta>>
            {
                { EstatusConsulta.Pendiente, new List<EstatusConsulta> { EstatusConsulta.EnProgreso, EstatusConsulta.Cancelada } },
                { EstatusConsulta.EnProgreso, new List<EstatusConsulta> { EstatusConsulta.Completada, EstatusConsulta.Cancelada } },
                { EstatusConsulta.Completada, new List<EstatusConsulta>() },
                { EstatusConsulta.Cancelada, new List<EstatusConsulta>() }

            };

            return transicionesValidas.ContainsKey(estadoActual) && transicionesValidas[estadoActual].Contains(nuevoEstado);
        }


        public async Task DeleteConsulta(int id)
        {
            try
            {
                await _consultaRepository.Delete(id);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(
                    message: "Error al eliminar consulta",
                    stackTrace: ex.StackTrace,
                    userId: null
                );
                throw;
            }
        }

        public async Task AssignMedicamentoToConsulta(int consultaId, int medicamentoId)
        {
            try
            {
                var consulta = await _consultaRepository.GetById(consultaId);
                if (consulta == null) throw new Exception("Consulta no encontrada");

                var medicamento = await _medicamentoRepository.GetById(medicamentoId);
                if (medicamento == null) throw new Exception("Medicamento no encontrado");

                if (!consulta.Medicamentos.Any(m => m.Id == medicamentoId))
                {
                    consulta.Medicamentos.Add(medicamento);
                    await _consultaRepository.Update(consulta);
                }
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(
                    message: "Error al asignar medicamento a consulta",
                    stackTrace: ex.StackTrace,
                    userId: null
                );
                throw;
            }
        }

        public async Task<IEnumerable<ConsultaDto>> GetConsultasByMascota(int mascotaId)
        {
            try
            {
                var consultas = await _consultaRepository.GetConsultasByMascotaId(mascotaId);
                return _mapper.Map<IEnumerable<ConsultaDto>>(consultas);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(
                    message: "Error al obtener consultas por mascota",
                    stackTrace: ex.StackTrace,
                    userId: null
                );
                throw;
            }
        }

        public async Task<IEnumerable<ConsultaDto>> GetConsultasByVeterinario(int veterinarioId)
        {
            try
            {
                var consultas = await _consultaRepository.GetConsultasByVeterinarioId(veterinarioId);
                return _mapper.Map<IEnumerable<ConsultaDto>>(consultas);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(
                    message: "Error al obtener consultas por veterinario",
                    stackTrace: ex.StackTrace,
                    userId: veterinarioId.ToString()
                );
                throw;
            }
        }
    }
}