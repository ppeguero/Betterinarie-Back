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

namespace Betterinarie_Back.Application.Services.Implementation
{
    public class MedicamentoService : IMedicamentoService
    {
        private readonly IRepository<Medicamento> _medicamentoRepository;
        private readonly IMapper _mapper;
        private readonly IErrorLogService _errorLogService;

        public MedicamentoService(IRepository<Medicamento> medicamentoRepository, IMapper mapper, IErrorLogService errorLogService)
        {
            _medicamentoRepository = medicamentoRepository;
            _mapper = mapper;
            _errorLogService = errorLogService;
        }

        public async Task<MedicamentoDto> GetMedicamentoById(int id)
        {
            try
            {
                var medicamento = await _medicamentoRepository.GetById(id, m => m.Consultas);
                if (medicamento == null) return null;
                return _mapper.Map<MedicamentoDto>(medicamento);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(
                    message: "Error al obtener medicamento por ID",
                    stackTrace: ex.StackTrace,
                    userId: null
                );
                throw;
            }
        }

        public async Task<IEnumerable<MedicamentoDto>> GetAllMedicamentos()
        {
            try
            {
                var medicamentos = await _medicamentoRepository.GetAll(m => m.Consultas);
                return _mapper.Map<IEnumerable<MedicamentoDto>>(medicamentos);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(
                    message: "Error al obtener todos los medicamentos",
                    stackTrace: ex.StackTrace,
                    userId: null
                );
                throw;
            }
        }

        public async Task<MedicamentoDto> CreateMedicamento(MedicamentoDto createDto)
        {
            try
            {
                var medicamento = _mapper.Map<Medicamento>(createDto);
                await _medicamentoRepository.Add(medicamento);
                return _mapper.Map<MedicamentoDto>(medicamento);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(
                    message: "Error al crear medicamento",
                    stackTrace: ex.StackTrace,
                    userId: null
                );
                throw;
            }
        }

        public async Task UpdateMedicamento(MedicamentoDto updateDto)
        {
            try
            {
                var medicamento = await _medicamentoRepository.GetById(updateDto.Id);
                if (medicamento == null) throw new Exception("Medicamento no encontrado");
                _mapper.Map(updateDto, medicamento);
                await _medicamentoRepository.Update(medicamento);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(
                    message: "Error al actualizar medicamento",
                    stackTrace: ex.StackTrace,
                    userId: null
                );
                throw;
            }
        }

        public async Task DeleteMedicamento(int id)
        {
            try
            {
                await _medicamentoRepository.Delete(id);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(
                    message: "Error al eliminar medicamento",
                    stackTrace: ex.StackTrace,
                    userId: null
                );
                throw;
            }
        }
    }
}