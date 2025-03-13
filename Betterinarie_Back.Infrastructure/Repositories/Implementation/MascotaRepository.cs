using Betterinarie_Back.Core.Entities.Implementation;
using Betterinarie_Back.Core.Interfaces.Implementation;
using Betterinarie_Back.Infrastructure.Data;
using Betterinarie_Back.Infrastructure.Repositories.RootBase;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betterinarie_Back.Infrastructure.Repositories.Implementation
{
    public class MascotaRepository : Repository<Mascota>, IMascotaRepository
    {
        public MascotaRepository(BetterinarieContext context) : base(context) { }

        public async Task<IEnumerable<Mascota>> GetMascotasByClienteId(int clienteId)
        {
            return await _context.Mascotas
                .Where(m => m.ClienteId == clienteId)
                .Include(m => m.Consultas)
                .ToListAsync();
        }
    }
}
