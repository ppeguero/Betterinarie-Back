using Betterinarie_Back.Application.Dtos.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betterinarie_Back.Application.Interfaces.Implementation
{
    public interface IRolService
    {
        Task<RolDto> GetRolById(int id);
        Task<IEnumerable<RolDto>> GetAllRoles();
        Task<RolDto> CreateRol(RolDto createDto);
        Task UpdateRol(RolDto updateDto);
        Task DeleteRol(int id);
    }
}
