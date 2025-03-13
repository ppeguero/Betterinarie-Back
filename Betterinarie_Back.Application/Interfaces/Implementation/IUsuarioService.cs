using Betterinarie_Back.Application.Dtos.Implementation;
using Betterinarie_Back.Application.Dtos.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betterinarie_Back.Application.Interfaces.Implementation
{
    public interface IUsuarioService
    {
        Task<UsuarioDto> GetUsuarioById(int id);
        Task<IEnumerable<UsuarioDto>> GetAllUsuarios();
        Task<UsuarioDto> CreateUsuario(RegisterDto createDto);
        Task UpdateUsuario(UsuarioDto updateDto);
        Task DeleteUsuario(int id);
        Task AssignRolToUsuario(int usuarioId, int rolId);
        Task<IEnumerable<ConsultaDto>> GetConsultasByVeterinario(int veterinarioId);
    }
}
