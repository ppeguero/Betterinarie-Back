using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Betterinarie_Back.Application.Dtos.Implementation;
using Betterinarie_Back.Application.Interfaces.Implementation;
using Betterinarie_Back.Application.Interfaces.Security;
using Betterinarie_Back.Core.Entities.Implementation;
using Betterinarie_Back.Core.Interfaces.Implementation;

namespace Betterinarie_Back.Application.Services.Implementation
{
    public class ClienteService : IClienteService
    {
        private readonly IRepository<Cliente> _clienteRepository;
        private readonly IMapper _mapper;
        private readonly IErrorLogService _errorLogService;

        public ClienteService(IRepository<Cliente> clienteRepository, IMapper mapper, IErrorLogService errorLogService)
        {
            _clienteRepository = clienteRepository;
            _mapper = mapper;
            _errorLogService = errorLogService;
        }

        public async Task<ClienteDto> GetClienteById(int id)
        {
            try
            {
                var dueño = await _clienteRepository.GetById(id, d => d.Mascotas);
                if (dueño == null) throw new Exception("Dueño no encontrado");
                return _mapper.Map<ClienteDto>(dueño);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(
                    message: "Error al obtener dueño por ID",
                    stackTrace: ex.StackTrace,
                    userId: null
                );
                throw;
            }
        }

        public async Task<IEnumerable<ClienteDto>> GetAllClientes()
        {
            try
            {
                var dueños = await _clienteRepository.GetAll(d => d.Mascotas);
                return _mapper.Map<IEnumerable<ClienteDto>>(dueños);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(
                    message: "Error al obtener todos los dueños",
                    stackTrace: ex.StackTrace,
                    userId: null
                );
                throw;
            }
        }

        public async Task<ClienteDto> CreateCliente(ClienteDto createDto)
        {
            try
            {
                var dueño = _mapper.Map<Cliente>(createDto);
                await _clienteRepository.Add(dueño);
                return _mapper.Map<ClienteDto>(dueño);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(
                    message: "Error al crear dueño",
                    stackTrace: ex.StackTrace,
                    userId: null
                );
                throw;
            }
        }

        public async Task UpdateCliente(ClienteDto updateDto)
        {
            try
            {
                var dueño = await _clienteRepository.GetById(updateDto.Id);
                if (dueño == null) throw new Exception("Dueño no encontrado");
                _mapper.Map(updateDto, dueño);
                await _clienteRepository.Update(dueño);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(
                    message: "Error al actualizar dueño",
                    stackTrace: ex.StackTrace,
                    userId: null
                );
                throw;
            }
        }

        public async Task DeleteCliente(int id)
        {
            try
            {
                await _clienteRepository.Delete(id);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(
                    message: "Error al eliminar dueño",
                    stackTrace: ex.StackTrace,
                    userId: null
                );
                throw;
            }
        }
    }
}
