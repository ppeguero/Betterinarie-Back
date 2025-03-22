using Betterinarie_Back.Core.Entities.Implementation;
using Betterinarie_Back.Core.Entities.Implementation.Enum;
using Betterinarie_Back.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

public class SeedData
{
    private readonly BetterinarieContext _context;
    private readonly UserManager<Usuario> _userManager;
    private readonly RoleManager<Rol> _roleManager;

    public SeedData(BetterinarieContext context, UserManager<Usuario> userManager, RoleManager<Rol> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task ExecuteSeed()
    {
        _context.Database.EnsureCreated();

        string[] roleNames = { "Administrador", "Veterinario", "Recepcionista" };
        foreach (var roleName in roleNames)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new Rol { Name = roleName });
            }
        }

        var users = new[]
        {
            new { Email = "admin@example.com", Nombre = "Administrador", Apellido = "User", Password = "Admin123!", Role = "Administrador" },
            new { Email = "vet@example.com", Nombre = "Veterinario", Apellido = "User", Password = "Vet123!", Role = "Veterinario" },
            new { Email = "recep@example.com", Nombre = "Recepcionista", Apellido = "User", Password = "Recep123!", Role = "Recepcionista" }
        };

        foreach (var userData in users)
        {
            var user = await _userManager.FindByEmailAsync(userData.Email);
            if (user == null)
            {
                user = new Usuario
                {
                    UserName = userData.Email,
                    Email = userData.Email,
                    Nombre = userData.Nombre,
                    Apellido = userData.Apellido
                };

                var result = await _userManager.CreateAsync(user, userData.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, userData.Role);
                }
                else
                {
                    throw new Exception($"Error al crear usuario {userData.Email}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }

        // 3. Sembrar clientes
        if (!_context.Clientes.Any())
        {
            var clientes = new[]
            {
                new Cliente { Nombre = "Juan", Apellido = "Perez", Direccion = "Calle Falsa 123", Telefono = "123456789" },
                new Cliente { Nombre = "Maria", Apellido = "Gonzalez", Direccion = "Avenida Siempreviva 742", Telefono = "987654321" }
            };
            _context.Clientes.AddRange(clientes);
            await _context.SaveChangesAsync(); // Guardar para generar IDs de clientes
        }

        // 4. Sembrar mascotas asociadas a clientes
        var cliente1 = _context.Clientes.FirstOrDefault(c => c.Nombre == "Juan" && c.Apellido == "Perez");
        var cliente2 = _context.Clientes.FirstOrDefault(c => c.Nombre == "Maria" && c.Apellido == "Gonzalez");

        if (cliente1 != null && cliente2 != null && !_context.Mascotas.Any())
        {
            var random = new Random();
            var hoy = DateTime.UtcNow.Date;
            var defaultImageUrl = "https://i.imgur.com/SDdEGJ9.png";


            var mascotas = new[]
            {
                new Mascota
                {
                    Nombre = "Firulais",
                    Especie = "Perro",
                    Raza = "Labrador",
                    FechaNacimiento = new DateTime(2020, 5, 10),
                    ClienteId = cliente1.Id,
                   FechaRegistro = hoy,
                   URLImagen = defaultImageUrl
                },
                new Mascota
                {
                    Nombre = "Michi",
                    Especie = "Gato",
                    Raza = "Siames",
                    FechaNacimiento = new DateTime(2019, 8, 15),
                    ClienteId = cliente1.Id,
                    FechaRegistro = hoy,
                    URLImagen = defaultImageUrl
                },
                new Mascota
                {
                    Nombre = "Bobby",
                    Especie = "Perro",
                    Raza = "Bulldog",
                    FechaNacimiento = new DateTime(2018, 3, 20),
                    ClienteId = cliente2.Id,
                    FechaRegistro = hoy,
                    URLImagen = defaultImageUrl
                },
                new Mascota
                {
                    Nombre = "Luna",
                    Especie = "Gato",
                    Raza = "Persa",
                    FechaNacimiento = new DateTime(2021, 1, 30),
                    ClienteId = cliente2.Id,
                   FechaRegistro = hoy,
                   URLImagen = defaultImageUrl
                },
                  new Mascota
                {
                    Nombre = "Luna",
                    Especie = "Gato",
                    Raza = "Persa",
                    FechaNacimiento = new DateTime(2021, 1, 30),
                    ClienteId = cliente1.Id,
                   FechaRegistro = hoy,
                   URLImagen = defaultImageUrl
                },
                   new Mascota
                {
                    Nombre = "Firulais",
                    Especie = "Perro",
                    Raza = "Labrador",
                    FechaNacimiento = new DateTime(2020, 5, 10),
                    ClienteId = cliente1.Id,
                   FechaRegistro = hoy,
                   URLImagen = defaultImageUrl
                },
                new Mascota
                {
                    Nombre = "Michi",
                    Especie = "Gato",
                    Raza = "Siames",
                    FechaNacimiento = new DateTime(2019, 8, 15),
                    ClienteId = cliente1.Id,
                    FechaRegistro = hoy,
                    URLImagen = defaultImageUrl
                },
                new Mascota
                {
                    Nombre = "Bobby",
                    Especie = "Perro",
                    Raza = "Bulldog",
                    FechaNacimiento = new DateTime(2018, 3, 20),
                    ClienteId = cliente2.Id,
                    FechaRegistro = hoy,
                    URLImagen = defaultImageUrl
                },
                new Mascota
                {
                    Nombre = "Luna",
                    Especie = "Gato",
                    Raza = "Persa",
                    FechaNacimiento = new DateTime(2021, 1, 30),
                    ClienteId = cliente2.Id,
                   FechaRegistro = hoy,
                   URLImagen = defaultImageUrl
                },
                  new Mascota
                {
                    Nombre = "Luna",
                    Especie = "Gato",
                    Raza = "Persa",
                    FechaNacimiento = new DateTime(2021, 1, 30),
                    ClienteId = cliente1.Id,
                   FechaRegistro = hoy,
                   URLImagen = defaultImageUrl
                },

            };
            _context.Mascotas.AddRange(mascotas);
            await _context.SaveChangesAsync(); 
        }

        if (!_context.Medicamentos.Any())
        {
            var medicamentos = new[]
            {
                new Medicamento { Nombre = "Antibiotico A", Descripcion = "Para infecciones bacterianas", Dosis = "10 mg/kg" },
                new Medicamento { Nombre = "Analgesico B", Descripcion = "Para el dolor moderado", Dosis = "5 mg/kg" },
                new Medicamento { Nombre = "Antiparasitario C", Descripcion = "Para eliminar parasitos internos", Dosis = "2 mg/kg" }
            };
            _context.Medicamentos.AddRange(medicamentos);
            await _context.SaveChangesAsync();
        }

        if (!_context.Consultas.Any())
        {
            var mascotas = _context.Mascotas.ToList();
            var veterinarios = await _userManager.GetUsersInRoleAsync("Veterinario");
            var hoy = DateTime.UtcNow.Date;

            if (mascotas.Any() && veterinarios.Any())
            {
                var consultas = new List<Consulta>
        {
            new Consulta
            {
                Fecha = hoy,
                Hora = new TimeOnly(10, 30),
                Estatus = EstatusConsulta.Pendiente,
                Motivo = "Vacunación",
                MascotaId = mascotas[0].Id,
                VeterinarioId = veterinarios[0].Id
            },
            new Consulta
            {
                Fecha = hoy.AddDays(1),
                Hora = new TimeOnly(14, 45),
                Estatus = EstatusConsulta.Completada,
                Motivo = "Chequeo general",
                MascotaId = mascotas[1].Id,
                VeterinarioId = veterinarios[0].Id
            },
            new Consulta
            {
                Fecha = hoy.AddDays(2),
                Hora = new TimeOnly(9, 15),
                Estatus = EstatusConsulta.Cancelada,
                Motivo = "Revisión por enfermedad",
                MascotaId = mascotas[2].Id,
                VeterinarioId = veterinarios[0].Id
            }
        };
                _context.Consultas.AddRange(consultas);
                await _context.SaveChangesAsync();
            }
        }

    }
}