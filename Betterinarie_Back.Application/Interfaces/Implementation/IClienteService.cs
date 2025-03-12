using Betterinarie_Back.Application.Dtos.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betterinarie_Back.Application.Interfaces.Implementation
{
    public interface IClienteService
    {
        Task<ClienteDto> GetClienteById(int id);
        Task<IEnumerable<ClienteDto>> GetAllClientes();
        Task<ClienteDto> CreateCliente(ClienteDto createDto);
        Task UpdateCliente(ClienteDto updateDto);
        Task DeleteCliente(int id);
    }
}
