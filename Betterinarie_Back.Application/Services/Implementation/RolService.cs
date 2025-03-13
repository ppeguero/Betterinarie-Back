using AutoMapper;
using Betterinarie_Back.Application.Dtos.Implementation;
using Betterinarie_Back.Application.Interfaces.Implementation;
using Betterinarie_Back.Application.Interfaces.Security;
using Betterinarie_Back.Core.Entities.Implementation;
using Betterinarie_Back.Core.Interfaces.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Betterinarie_Back.Application.Services.Implementation
{
    public class RolService : IRolService
    {
        private readonly IRepository<Rol> _rolRepository;
        private readonly IMapper _mapper;
        private readonly IErrorLogService _errorLogService;

        public RolService(IRepository<Rol> rolRepository, IMapper mapper, IErrorLogService errorLogService)
        {
            _rolRepository = rolRepository;
            _mapper = mapper;
            _errorLogService = errorLogService;
        }

        public async Task<RolDto> GetRolById(int id)
        {
            try
            {
                var rol = await _rolRepository.GetById(id);
                return _mapper.Map<RolDto>(rol);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(
                    message: "Error al obtener rol por ID",
                    stackTrace: ex.StackTrace,
                    userId: null
                );
                throw;
            }
        }

        public async Task<IEnumerable<RolDto>> GetAllRoles()
        {
            try
            {
                var roles = await _rolRepository.GetAll();
                return _mapper.Map<IEnumerable<RolDto>>(roles);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(
                    message: "Error al obtener todos los roles",
                    stackTrace: ex.StackTrace,
                    userId: null
                );
                throw;
            }
        }

        public async Task<RolDto> CreateRol(RolDto createDto)
        {
            try
            {
                var rol = _mapper.Map<Rol>(createDto);
                await _rolRepository.Add(rol);
                return _mapper.Map<RolDto>(rol);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(
                    message: "Error al crear rol",
                    stackTrace: ex.StackTrace,
                    userId: null
                );
                throw;
            }
        }

        public async Task UpdateRol(RolDto updateDto)
        {
            try
            {
                var rol = await _rolRepository.GetById(updateDto.Id);
                if (rol == null) throw new Exception("Rol no encontrado");
                _mapper.Map(updateDto, rol);
                await _rolRepository.Update(rol);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(
                    message: "Error al actualizar rol",
                    stackTrace: ex.StackTrace,
                    userId: null
                );
                throw;
            }
        }

        public async Task DeleteRol(int id)
        {
            try
            {
                await _rolRepository.Delete(id);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(
                    message: "Error al eliminar rol",
                    stackTrace: ex.StackTrace,
                    userId: null
                );
                throw;
            }
        }


    }
}