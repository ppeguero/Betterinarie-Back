using Betterinarie_Back.Core.Entities.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betterinarie_Back.Core.Interfaces.Implementation
{
    public interface IMascotaRepository : IRepository<Mascota>
    {
        //Task<IEnumerable<Mascota>> GetMascotasByClienteId(int clienteId);
        Task<IEnumerable<Consulta>> GetHistorialCitas(int mascotaId);

    }
}
