using Betterinarie_Back.Core.Entities.Implementation;
using Betterinarie_Back.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

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
        // Asegurar que la base de datos esté creada
        _context.Database.EnsureCreated();

        // 1. Sembrar roles
        string[] roleNames = { "Administrador", "Veterinario", "Recepcionista" };
        foreach (var roleName in roleNames)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new Rol { Name = roleName });
            }
        }

        // 2. Sembrar usuarios
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
            await _context.SaveChangesAsync();
        }

        // 4. Sembrar mascotas asociadas a clientes
        var cliente1 = _context.Clientes.FirstOrDefault(c => c.Nombre == "Juan" && c.Apellido == "Perez");
        var cliente2 = _context.Clientes.FirstOrDefault(c => c.Nombre == "Maria" && c.Apellido == "Gonzalez");

        if (cliente1 != null && cliente2 != null && !_context.Mascotas.Any())
        {
            var mascotas = new[]
             {
                new Mascota
                {
                    Nombre = "Firulais",
                    Especie = "Perro",
                    Raza = "Labrador",
                    FechaNacimiento = new DateTime(2020, 5, 10),
                    ClienteId = cliente1.Id,
                    FechaRegistro = new DateTime(2023, 1, 1, 12, 0, 0, DateTimeKind.Utc) // Fecha específica
                },
                new Mascota
                {
                    Nombre = "Michi",
                    Especie = "Gato",
                    Raza = "Siames",
                    FechaNacimiento = new DateTime(2019, 8, 15),
                    ClienteId = cliente1.Id,
                    FechaRegistro = new DateTime(2023, 2, 1, 12, 0, 0, DateTimeKind.Utc)
                },
                new Mascota
                {
                    Nombre = "Bobby",
                    Especie = "Perro",
                    Raza = "Bulldog",
                    FechaNacimiento = new DateTime(2018, 3, 20),
                    ClienteId = cliente2.Id,
                    FechaRegistro = new DateTime(2023, 3, 1, 12, 0, 0, DateTimeKind.Utc)
                },
                new Mascota
                {
                    Nombre = "Luna",
                    Especie = "Gato",
                    Raza = "Persa",
                    FechaNacimiento = new DateTime(2021, 1, 30),
                    ClienteId = cliente2.Id,
                    FechaRegistro = new DateTime(2023, 4, 1, 12, 0, 0, DateTimeKind.Utc)
                }
            };
            _context.Mascotas.AddRange(mascotas);
            await _context.SaveChangesAsync();
        }

        // 5. Sembrar medicamentos
        if (!_context.Medicamentos.Any())
        {
            var medicamentos = new[]
            {
                new Medicamento { Nombre = "Antibiotico A", Descripcion = "Para infecciones bacterianas" },
                new Medicamento { Nombre = "Analgesico B", Descripcion = "Para el dolor moderado" },
                new Medicamento { Nombre = "Antiparasitario C", Descripcion = "Para eliminar parasitos internos" }
            };
            _context.Medicamentos.AddRange(medicamentos);
            await _context.SaveChangesAsync();
        }
    }
}
