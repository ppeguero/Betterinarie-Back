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
    public class ClienteRepository : Repository<Cliente>, IClienteRepository
    {
        public ClienteRepository(BetterinarieContext context) : base(context) { }


        public async Task UpdateClienteWithMascotasAsync(Cliente cliente, IEnumerable<int> mascotasIds)
        {
            var existingCliente = await _context.Clientes
                .Include(c => c.Mascotas)
                .FirstOrDefaultAsync(c => c.Id == cliente.Id);

            if (existingCliente == null)
                throw new Exception("Cliente no encontrado");

            existingCliente.Nombre = cliente.Nombre;
            existingCliente.Apellido = cliente.Apellido;
            existingCliente.Direccion = cliente.Direccion;
            existingCliente.Telefono = cliente.Telefono;

            var mascotasActuales = existingCliente.Mascotas.ToList();
            var mascotasNuevasIds = mascotasIds ?? new List<int>(); 

            var mascotasToRemove = mascotasActuales
                .Where(m => !mascotasNuevasIds.Contains(m.Id))
                .ToList();
            foreach (var mascota in mascotasToRemove)
            {
                existingCliente.Mascotas.Remove(mascota);
            }

            var mascotasExistentesIds = mascotasActuales.Select(m => m.Id).ToList();
            var mascotasAAgregarIds = mascotasNuevasIds.Except(mascotasExistentesIds).ToList();

            foreach (var mascotaId in mascotasAAgregarIds)
            {
                var mascota = await _context.Mascotas.FindAsync(mascotaId);
                if (mascota != null)
                {
                    existingCliente.Mascotas.Add(mascota);
                }
                else
                {
                    throw new Exception($"Mascota con ID {mascotaId} no encontrada");
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteClienteWithMascotasAsync(int id)
        {
            var cliente = await _context.Clientes
                .Include(c => c.Mascotas)
                .ThenInclude(m => m.Consultas)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cliente == null)
                throw new Exception("Cliente no encontrado");

            foreach (var mascota in cliente.Mascotas) {
                _context.Consultas.RemoveRange(mascota.Consultas);
            }

            _context.Mascotas.RemoveRange(cliente.Mascotas);
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
        }
    }
}
