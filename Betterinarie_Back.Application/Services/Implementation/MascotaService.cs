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
    public class MascotaService : IMascotaService
    {
        private readonly IRepository<Mascota> _petRepository;
        private readonly IMascotaRepository _mascotaRepository;
        private readonly IMapper _mapper;
        private readonly IErrorLogService _errorLogService;

        public MascotaService(IMascotaRepository mascotaRepository, IRepository<Mascota> petRepository, IMapper mapper, IErrorLogService errorLogService)
        {
            _mascotaRepository = mascotaRepository;
            _petRepository = petRepository;
            _mapper = mapper;
            _errorLogService = errorLogService;
        }

        public async Task<MascotaDto> GetMascotaById(int id)
        {
            try
            {
                var mascota = await _petRepository.GetById(id, m => m.Consultas);
                return _mapper.Map<MascotaDto>(mascota);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(
                    message: "Error al obtener mascota por ID",
                    stackTrace: ex.StackTrace,
                    userId: null
                );
                throw;
            }
        }

        public async Task<IEnumerable<MascotaDto>> GetAllMascotas()
        {
            try
            {
                var mascotas = await _mascotaRepository.GetAll(m => m.Consultas);
                return _mapper.Map<IEnumerable<MascotaDto>>(mascotas);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(
                    message: "Error al obtener todas las mascotas",
                    stackTrace: ex.StackTrace,
                    userId: null
                );
                throw;
            }
        }

        public async Task<MascotaDto> CreateMascota(MascotaDto createDto)
        {
            try
            {
                var mascota = _mapper.Map<Mascota>(createDto);
                await _mascotaRepository.Add(mascota);
                return _mapper.Map<MascotaDto>(mascota);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(
                    message: "Error al crear mascota",
                    stackTrace: ex.StackTrace,
                    userId: null
                );
                throw;
            }
        }

        public async Task UpdateMascota(MascotaDto updateDto)
        {
            try
            {
                var mascota = await _mascotaRepository.GetById(updateDto.Id);
                if (mascota == null) throw new Exception("Mascota no encontrada");
                _mapper.Map(updateDto, mascota);
                await _mascotaRepository.Update(mascota);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(
                    message: "Error al actualizar mascota",
                    stackTrace: ex.StackTrace,
                    userId: null
                );
                throw;
            }
        }

        public async Task DeleteMascota(int id)
        {
            try
            {
                await _mascotaRepository.Delete(id);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(
                    message: "Error al eliminar mascota",
                    stackTrace: ex.StackTrace,
                    userId: null
                );
                throw;
            }
        }


        public async Task<IEnumerable<MascotaDto>> GetMascotasByCliente(int clienteId)
        {
            try
            {
                var mascotas = await _mascotaRepository.GetMascotasByClienteId(clienteId);
                return _mapper.Map<IEnumerable<MascotaDto>>(mascotas);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(
                    message: "Error al obtener mascotas por dueño",
                    stackTrace: ex.StackTrace,
                    userId: clienteId.ToString()
                );
                throw;
            }
        }
    }
}