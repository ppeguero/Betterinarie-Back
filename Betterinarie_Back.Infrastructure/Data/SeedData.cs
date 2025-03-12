using Betterinarie_Back.Core.Entities.Implementation;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Betterinarie_Back.Infrastructure.Data
{
    public static class SeedData
    {
        public static async Task Seed(BetterinarieContext context, UserManager<Usuario> userManager, RoleManager<Rol> roleManager)
        {
            // Asegurar que la base de datos esté creada
            context.Database.EnsureCreated();

            // 1. Sembrar roles
            string[] roleNames = { "Administrador", "Veterinario", "Recepcionista" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new Rol { Name = roleName });
                }
            }

            // 2. Sembrar usuarios
            var users = new[]
            {
                new { Email = "admin@example.com", Nombre = "Admin", Apellido = "User", Password = "Admin123!", Role = "Administrador" },
                new { Email = "vet@example.com", Nombre = "Vet", Apellido = "User", Password = "Vet123!", Role = "Veterinario" },
                new { Email = "recep@example.com", Nombre = "Recep", Apellido = "User", Password = "Recep123!", Role = "Recepcionista" }
            };

            foreach (var userData in users)
            {
                var user = await userManager.FindByEmailAsync(userData.Email);
                if (user == null)
                {
                    user = new Usuario
                    {
                        UserName = userData.Email,
                        Email = userData.Email,
                        Nombre = userData.Nombre,
                        Apellido = userData.Apellido
                    };

                    var result = await userManager.CreateAsync(user, userData.Password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, userData.Role);
                    }
                    else
                    {
                        throw new Exception($"Error al crear usuario {userData.Email}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                }
            }

            // 3. Sembrar clientes
            if (!context.Clientes.Any())
            {
                var clientes = new[]
                {
                    new Cliente { Nombre = "Juan", Apellido = "Perez", Direccion = "Calle Falsa 123", Telefono = "123456789" },
                    new Cliente { Nombre = "Maria", Apellido = "Gonzalez", Direccion = "Avenida Siempreviva 742", Telefono = "987654321" }
                };
                context.Clientes.AddRange(clientes);
                await context.SaveChangesAsync();
            }

            // 4. Sembrar mascotas asociadas a clientes
            var cliente1 = context.Clientes.FirstOrDefault(c => c.Nombre == "Juan" && c.Apellido == "Perez");
            var cliente2 = context.Clientes.FirstOrDefault(c => c.Nombre == "Maria" && c.Apellido == "Gonzalez");

            if (cliente1 != null && cliente2 != null && !context.Mascotas.Any())
            {
                var mascotas = new[]
                {
                    new Mascota { Nombre = "Firulais", Especie = "Perro", Raza = "Labrador", FechaNacimiento = new DateTime(2020, 5, 10), ClienteId = cliente1.Id },
                    new Mascota { Nombre = "Michi", Especie = "Gato", Raza = "Siames", FechaNacimiento = new DateTime(2019, 8, 15), ClienteId = cliente1.Id },
                    new Mascota { Nombre = "Bobby", Especie = "Perro", Raza = "Bulldog", FechaNacimiento = new DateTime(2018, 3, 20), ClienteId = cliente2.Id },
                    new Mascota { Nombre = "Luna", Especie = "Gato", Raza = "Persa", FechaNacimiento = new DateTime(2021, 1, 30), ClienteId = cliente2.Id }
                };
                context.Mascotas.AddRange(mascotas);
                await context.SaveChangesAsync();
            }

            // 5. Sembrar medicamentos
            if (!context.Medicamentos.Any())
            {
                var medicamentos = new[]
                {
                    new Medicamento { Nombre = "Antibiotico A", Descripcion = "Para infecciones bacterianas" },
                    new Medicamento { Nombre = "Analgesico B", Descripcion = "Para el dolor moderado" },
                    new Medicamento { Nombre = "Antiparasitario C", Descripcion = "Para eliminar parasitos internos" }
                };
                context.Medicamentos.AddRange(medicamentos);
                await context.SaveChangesAsync();
            }
        }
    }
}