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
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IMapper _mapper;
        private readonly IErrorLogService _errorLogService;

        public ClienteService(IClienteRepository clienteRepository, IMapper mapper, IErrorLogService errorLogService)
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

        public async Task UpdateCliente(ClienteUpdateDto updateDto)
        {
            try
            {
                var cliente = await _clienteRepository.GetById(updateDto.Id, c => c.Mascotas);
                if (cliente == null) throw new Exception("Cliente no encontrado");

                // Asignación manual de campos
                cliente.Nombre = updateDto.Nombre;
                cliente.Apellido = updateDto.Apellido;
                cliente.Direccion = updateDto.Direccion;
                cliente.Telefono = updateDto.Telefono;

                // Llamar al repositorio para sincronizar mascotas y guardar
                await _clienteRepository.UpdateClienteWithMascotasAsync(cliente, updateDto.MascotasIds);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(
                    message: "Error al actualizar cliente",
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
                await _clienteRepository.DeleteClienteWithMascotasAsync(id);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(message: "Error al eliminar cliente",
                   stackTrace: ex.StackTrace,
                   userId: null
               );
                throw;
            }
        }
    }
}