using Betterinarie_Back.Application.Dtos.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betterinarie_Back.Application.Interfaces.Implementation
{
    public interface IMascotaService
    {
        Task<MascotaDto> GetMascotaById(int id);
        Task<IEnumerable<MascotaDto>> GetAllMascotas();
        Task<MascotaDto> CreateMascota(MascotaDto createDto);
        Task UpdateMascota(MascotaDto updateDto);
        Task DeleteMascota(int id);
        Task<IEnumerable<MascotaDto>> GetMascotasByCliente(int clienteId);
    }
}
