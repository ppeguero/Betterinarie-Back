using Betterinarie_Back.Core.Entities.Implementation;
using Betterinarie_Back.Core.Entities.Implementation.Enum;
using Betterinarie_Back.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using CloudinaryDotNet.Actions;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Betterinarie_Back.Core.Interfaces.Data;

public class SeedData
{
    private readonly BetterinarieContext _context;
    private readonly UserManager<Usuario> _userManager;
    private readonly RoleManager<Rol> _roleManager;
    private readonly ICloudinaryService _cloudinaryService;
    private readonly string _wwwrootPath;

    public SeedData(BetterinarieContext context, UserManager<Usuario> userManager, RoleManager<Rol> roleManager, ICloudinaryService cloudinaryService)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _cloudinaryService = cloudinaryService;
        // Calcular la ruta a wwwroot internamente
        var basePath = Directory.GetCurrentDirectory();
        _wwwrootPath = Path.Combine(basePath, "wwwroot");
        Console.WriteLine($"Ruta de wwwroot: {_wwwrootPath}");
    }

    public async Task ExecuteSeed()
    {
        try
        {
            Console.WriteLine("Iniciando siembra de datos...");

            _context.Database.EnsureCreated();
            Console.WriteLine("Base de datos asegurada.");

            // Crear roles
            string[] roleNames = { "Administrador", "Veterinario", "Recepcionista" };
            foreach (var roleName in roleNames)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new Rol { Name = roleName });
                    Console.WriteLine($"Rol creado: {roleName}");
                }
                else
                {
                    Console.WriteLine($"Rol ya existe: {roleName}");
                }
            }

            // Crear usuarios
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
                        Console.WriteLine($"Usuario creado: {userData.Email} con rol {userData.Role}");
                    }
                    else
                    {
                        throw new Exception($"Error al crear usuario {userData.Email}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                }
                else
                {
                    Console.WriteLine($"Usuario ya existe: {userData.Email}");
                }
            }

            // Crear clientes
            if (!_context.Clientes.Any())
            {
                var clientes = new[]
                {
                    new Cliente { Nombre = "Juan", Apellido = "Perez", Direccion = "Calle Falsa 123", Telefono = "123456789" },
                    new Cliente { Nombre = "Maria", Apellido = "Gonzalez", Direccion = "Avenida Siempreviva 742", Telefono = "987654321" }
                };
                _context.Clientes.AddRange(clientes);
                await _context.SaveChangesAsync();
                Console.WriteLine("Clientes creados.");
            }
            else
            {
                Console.WriteLine("Clientes ya existen.");
            }

            var cliente1 = _context.Clientes.FirstOrDefault(c => c.Nombre == "Juan" && c.Apellido == "Perez");
            var cliente2 = _context.Clientes.FirstOrDefault(c => c.Nombre == "Maria" && c.Apellido == "Gonzalez");

            if (cliente1 == null || cliente2 == null)
            {
                Console.WriteLine("Error: No se encontraron los clientes esperados.");
                return;
            }

            if (!_context.Mascotas.Any())
            {
                Console.WriteLine("Creando mascotas...");
                var hoy = DateTime.UtcNow.Date;

                var mascotas = new[]
                {
                    new Mascota { Nombre = "Firulais", Especie = "Perro", Raza = "Labrador", FechaNacimiento = new DateTime(2020, 5, 10), ClienteId = cliente1.Id, FechaRegistro = hoy, URLImagen = "/mascotas/mascota1.jpg" },
                    new Mascota { Nombre = "Michi", Especie = "Gato", Raza = "Siames", FechaNacimiento = new DateTime(2019, 8, 15), ClienteId = cliente1.Id, FechaRegistro = hoy, URLImagen = "/mascotas/mascota2.jpg" },
                    new Mascota { Nombre = "Bobby", Especie = "Perro", Raza = "Bulldog", FechaNacimiento = new DateTime(2018, 3, 20), ClienteId = cliente2.Id, FechaRegistro = hoy, URLImagen = "/mascotas/mascota3.jpg" },
                    new Mascota { Nombre = "Luna", Especie = "Gato", Raza = "Persa", FechaNacimiento = new DateTime(2021, 1, 30), ClienteId = cliente2.Id, FechaRegistro = hoy, URLImagen = "/mascotas/mascota4.jpg" },
                    new Mascota { Nombre = "Pelusa", Especie = "Conejo", Raza = "Holandés", FechaNacimiento = new DateTime(2022, 4, 12), ClienteId = cliente1.Id, FechaRegistro = hoy, URLImagen = "/mascotas/mascota5.jpg" },
                    new Mascota { Nombre = "Toby", Especie = "Perro", Raza = "Chihuahua", FechaNacimiento = new DateTime(2022, 6, 15), ClienteId = cliente2.Id, FechaRegistro = hoy, URLImagen = "/mascotas/mascota6.jpg" },
                    new Mascota { Nombre = "Simba", Especie = "Gato", Raza = "Maine Coon", FechaNacimiento = new DateTime(2020, 9, 25), ClienteId = cliente1.Id, FechaRegistro = hoy, URLImagen = "/mascotas/mascota7.jpg" },
                    new Mascota { Nombre = "Nibbles", Especie = "Hamster", Raza = "Sirio", FechaNacimiento = new DateTime(2023, 2, 10), ClienteId = cliente2.Id, FechaRegistro = hoy, URLImagen = "/mascotas/mascota8.jpg" },
                    new Mascota { Nombre = "Coco", Especie = "Perro", Raza = "Golden Retriever", FechaNacimiento = new DateTime(2021, 3, 18), ClienteId = cliente1.Id, FechaRegistro = hoy, URLImagen = "/mascotas/mascota9.jpg" },
                    new Mascota { Nombre = "Piolín", Especie = "Pájaro", Raza = "Canario", FechaNacimiento = new DateTime(2021, 11, 5), ClienteId = cliente2.Id, FechaRegistro = hoy, URLImagen = "/mascotas/mascota10.jpg" },
                    new Mascota { Nombre = "Whiskers", Especie = "Gato", Raza = "Ragdoll", FechaNacimiento = new DateTime(2020, 2, 14), ClienteId = cliente1.Id, FechaRegistro = hoy, URLImagen = "/mascotas/mascota11.jpg" },
                    new Mascota { Nombre = "Rex", Especie = "Perro", Raza = "Rottweiler", FechaNacimiento = new DateTime(2019, 11, 30), ClienteId = cliente2.Id, FechaRegistro = hoy, URLImagen = "/mascotas/mascota12.jpg" },
                    new Mascota { Nombre = "Fluffy", Especie = "Conejo", Raza = "Angora", FechaNacimiento = new DateTime(2021, 7, 20), ClienteId = cliente1.Id, FechaRegistro = hoy, URLImagen = "/mascotas/mascota13.jpg" },
                    new Mascota { Nombre = "Chispa", Especie = "Hamster", Raza = "Roborovski", FechaNacimiento = new DateTime(2022, 12, 8), ClienteId = cliente2.Id, FechaRegistro = hoy, URLImagen = "/mascotas/mascota14.jpg" },
                    new Mascota { Nombre = "Loro", Especie = "Pájaro", Raza = "Loro Gris", FechaNacimiento = new DateTime(2018, 10, 15), ClienteId = cliente1.Id, FechaRegistro = hoy, URLImagen = "/mascotas/mascota15.jpg" }
                };

                // Subir imágenes a Cloudinary
                foreach (var mascota in mascotas)
                {
                    var filePath = Path.Combine(_wwwrootPath, "mascotas", Path.GetFileName(mascota.URLImagen.TrimStart('/')));
                    if (File.Exists(filePath))
                    {
                        try
                        {
                            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                            {
                                Console.WriteLine($"Subiendo imagen para {mascota.Nombre} desde {filePath}");
                                var publicId = $"mascotas/{mascota.Nombre}";
                                var uploadResult = await _cloudinaryService.UploadImage(stream, "mascotas", mascota.Nombre);

                                if (uploadResult != null && uploadResult.PublicId != null)
                                {
                                    mascota.URLImagen = uploadResult.SecureUrl.ToString();
                                    mascota.PublicIdImagen = uploadResult.PublicId;
                                    Console.WriteLine($"Imagen subida para {mascota.Nombre}: PublicId={mascota.PublicIdImagen}, URL={mascota.URLImagen}");
                                }
                                else
                                {
                                    Console.WriteLine($"Error: No se recibió PublicId para {mascota.Nombre}. Resultado: {uploadResult?.Error?.Message ?? "Resultado nulo"}");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error al subir imagen para {mascota.Nombre}: {ex.Message}");
                            Console.WriteLine($"StackTrace: {ex.StackTrace}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Imagen no encontrada: {filePath}");
                    }
                }

                _context.Mascotas.AddRange(mascotas);
                await _context.SaveChangesAsync();
                Console.WriteLine("Mascotas creadas y guardadas.");
            }
            else
            {
                Console.WriteLine("Mascotas ya existen.");
            }

            // Crear medicamentos
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
                Console.WriteLine("Medicamentos creados.");
            }
            else
            {
                Console.WriteLine("Medicamentos ya existen.");
            }

            // Crear consultas
            if (!_context.Consultas.Any())
            {
                var mascotas = _context.Mascotas.ToList();
                var veterinarios = await _userManager.GetUsersInRoleAsync("Veterinario");
                var hoy = DateTime.UtcNow.Date;

                if (mascotas.Any() && veterinarios.Any())
                {
                    var consultas = new List<Consulta>
                    {
                        new Consulta { Fecha = hoy, Hora = new TimeOnly(10, 30), Estatus = EstatusConsulta.Pendiente, Motivo = "Vacunación", MascotaId = mascotas[0].Id, VeterinarioId = veterinarios[0].Id },
                        new Consulta { Fecha = hoy.AddDays(1), Hora = new TimeOnly(14, 45), Estatus = EstatusConsulta.Completada, Motivo = "Chequeo general", MascotaId = mascotas[1].Id, VeterinarioId = veterinarios[0].Id },
                        new Consulta { Fecha = hoy.AddDays(2), Hora = new TimeOnly(9, 15), Estatus = EstatusConsulta.Cancelada, Motivo = "Revisión por enfermedad", MascotaId = mascotas[2].Id, VeterinarioId = veterinarios[0].Id }
                    };
                    _context.Consultas.AddRange(consultas);
                    await _context.SaveChangesAsync();
                    Console.WriteLine("Consultas creadas.");
                }
                else
                {
                    Console.WriteLine("No se crearon consultas: No hay mascotas o veterinarios.");
                }
            }
            else
            {
                Console.WriteLine("Consultas ya existen.");
            }

            Console.WriteLine("Siembra de datos completada.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error durante la siembra de datos: {ex.Message}");
            Console.WriteLine($"StackTrace: {ex.StackTrace}");
            throw;
        }
    }
}