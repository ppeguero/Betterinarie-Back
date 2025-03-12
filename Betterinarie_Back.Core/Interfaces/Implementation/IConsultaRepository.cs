using Betterinarie_Back.Core.Entities.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betterinarie_Back.Core.Interfaces.Implementation
{
    public interface IConsultaRepository : IRepository<Consulta>
    {
        Task<IEnumerable<Consulta>> GetConsultasByMascotaId(int mascotaId);
        Task<IEnumerable<Consulta>> GetConsultasByVeterinarioId(int veterinarioId);
    }
}
