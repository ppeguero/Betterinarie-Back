using Betterinarie_Back.Core.Entities.Implementation;
using Betterinarie_Back.Core.Entities.Security;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betterinarie_Back.Infrastructure.Data
{
    public class BetterinarieContext : IdentityDbContext<Usuario, Rol, int>
    {
        public BetterinarieContext(DbContextOptions<BetterinarieContext> options): base(options) { }

        public DbSet<Mascota> Mascotas { get; set; }
        public DbSet<Consulta> Consultas { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Medicamento> Medicamentos { get; set; }
        public DbSet<LogError> LogErrors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Cliente>().ToTable("Cliente");
        }
    }
}
