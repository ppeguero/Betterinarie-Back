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
    public class ConsultaRepository : Repository<Consulta>, IConsultaRepository
    {
        public ConsultaRepository(BetterinarieContext context) : base(context) { }

        public async Task<IEnumerable<Consulta>> GetConsultasByMascotaId(int mascotaId)
        {
            return await _context.Consultas
                .Where(c => c.MascotaId == mascotaId)
                .Include(c => c.Medicamentos)
            .ToListAsync();
        }

        public async Task<IEnumerable<Consulta>> GetConsultasByVeterinarioId(int veterinarioId)
        {
            return await _context.Consultas
                .Where(c => c.VeterinarioId == veterinarioId)
                .Include(c => c.Medicamentos)
                .ToListAsync();
        }
    }
}
