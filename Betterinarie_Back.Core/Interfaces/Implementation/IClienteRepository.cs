using Betterinarie_Back.Core.Entities.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betterinarie_Back.Core.Interfaces.Implementation
{
    public interface IClienteRepository : IRepository<Cliente>
    {

        Task UpdateClienteWithMascotasAsync(Cliente cliente, IEnumerable<int> mascotasIds);
        Task DeleteClienteWithMascotasAsync(int id);
    }
}
