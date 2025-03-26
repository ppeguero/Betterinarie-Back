using Betterinarie_Back.Core.Entities.Implementation;
using Betterinarie_Back.Core.Entities.Implementation.Enum;
using Betterinarie_Back.Core.Interfaces.Data;
using Betterinarie_Back.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;

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

            var users = new[]
            {
              new { Email = "pipa@betterinariesystem.com", Nombre = "Pipa", Apellido = "Administradora", Password = "Admin123!", Role = "Administrador" },
              new { Email = "jose@betterinariesystem.com", Nombre = "José", Apellido = "Martinez", Password = "Admin123!", Role = "Veterinario" },
              new { Email = "pech@betterinariesystem.com", Nombre = "Pech", Apellido = "Recepcionista", Password = "Vet123!", Role = "Recepcionista" },
              new { Email = "eduardo@betterinariesystem.com", Nombre = "Eduardo", Apellido = "Veterinario", Password = "Recep123!", Role = "Veterinario" }
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

            if (!_context.Clientes.Any())
            {
                var clientes = new[]
                {
                  new Cliente { Nombre = "Juan", Apellido = "Perez", Direccion = "Calle Falsa 123", Telefono = "123456789" },
                    new Cliente { Nombre = "Maria", Apellido = "Gonzalez", Direccion = "Avenida Siempreviva 742", Telefono = "987654321" },
                    new Cliente { Nombre = "Carlos", Apellido = "Ramirez", Direccion = "Boulevard Central 500", Telefono = "555123456" },
                    new Cliente { Nombre = "Ana", Apellido = "Lopez", Direccion = "Calle del Sol 321", Telefono = "333987654" },
                    new Cliente { Nombre = "Pedro", Apellido = "Sanchez", Direccion = "Avenida del Mar 100", Telefono = "444567890" },
                    new Cliente { Nombre = "Sofia", Apellido = "Martinez", Direccion = "Calle Luna 55", Telefono = "666234567" },
                    new Cliente { Nombre = "Luis", Apellido = "Fernandez", Direccion = "Paseo de la Reforma 890", Telefono = "777876543" },
                    new Cliente { Nombre = "Elena", Apellido = "Gutierrez", Direccion = "Carrera 45 #12-34", Telefono = "888345678" },
                    new Cliente { Nombre = "Miguel", Apellido = "Torres", Direccion = "Calle Primavera 567", Telefono = "999654321" },
                    new Cliente { Nombre = "Camila", Apellido = "Rojas", Direccion = "Avenida Libertad 789", Telefono = "111987654" }
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
            var cliente3 = _context.Clientes.FirstOrDefault(c => c.Nombre == "Carlos" && c.Apellido == "Ramirez");
            var cliente4 = _context.Clientes.FirstOrDefault(c => c.Nombre == "Ana" && c.Apellido == "Lopez");
            var cliente5 = _context.Clientes.FirstOrDefault(c => c.Nombre == "Pedro" && c.Apellido == "Sanchez");
            var cliente6 = _context.Clientes.FirstOrDefault(c => c.Nombre == "Sofia" && c.Apellido == "Martinez");
            var cliente7 = _context.Clientes.FirstOrDefault(c => c.Nombre == "Luis" && c.Apellido == "Fernandez");
            var cliente8 = _context.Clientes.FirstOrDefault(c => c.Nombre == "Elena" && c.Apellido == "Gutierrez");
            var cliente9 = _context.Clientes.FirstOrDefault(c => c.Nombre == "Miguel" && c.Apellido == "Torres");
            var cliente10 = _context.Clientes.FirstOrDefault(c => c.Nombre == "Camila" && c.Apellido == "Rojas");


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

                    new Mascota { Nombre = "Pelusa", Especie = "Conejo", Raza = "Holandés", FechaNacimiento = new DateTime(2022, 4, 12), ClienteId = cliente3.Id, FechaRegistro = hoy, URLImagen = "/mascotas/mascota5.jpg" },

                    new Mascota { Nombre = "Toby", Especie = "Perro", Raza = "Chihuahua", FechaNacimiento = new DateTime(2022, 6, 15), ClienteId = cliente4.Id, FechaRegistro = hoy, URLImagen = "/mascotas/mascota6.jpg" },

                    new Mascota { Nombre = "Simba", Especie = "Gato", Raza = "Maine Coon", FechaNacimiento = new DateTime(2020, 9, 25), ClienteId = cliente5.Id, FechaRegistro = hoy, URLImagen = "/mascotas/mascota7.jpg" },

                    new Mascota { Nombre = "Nibbles", Especie = "Hamster", Raza = "Sirio", FechaNacimiento = new DateTime(2023, 2, 10), ClienteId = cliente6.Id, FechaRegistro = hoy, URLImagen = "/mascotas/mascota8.jpg" },

                    new Mascota { Nombre = "Coco", Especie = "Perro", Raza = "Golden Retriever", FechaNacimiento = new DateTime(2021, 3, 18), ClienteId = cliente7.Id, FechaRegistro = hoy, URLImagen = "/mascotas/mascota9.jpg" },

                    new Mascota { Nombre = "Piolín", Especie = "Pájaro", Raza = "Canario", FechaNacimiento = new DateTime(2021, 11, 5), ClienteId = cliente8.Id, FechaRegistro = hoy, URLImagen = "/mascotas/mascota10.jpg" },

                    new Mascota { Nombre = "Whiskers", Especie = "Gato", Raza = "Ragdoll", FechaNacimiento = new DateTime(2020, 2, 14), ClienteId = cliente9.Id, FechaRegistro = hoy, URLImagen = "/mascotas/mascota11.jpg" },

                    new Mascota { Nombre = "Rex", Especie = "Perro", Raza = "Rottweiler", FechaNacimiento = new DateTime(2019, 11, 30), ClienteId = cliente10.Id, FechaRegistro = hoy, URLImagen = "/mascotas/mascota12.jpg" },

                    new Mascota { Nombre = "Fluffy", Especie = "Conejo", Raza = "Angora", FechaNacimiento = new DateTime(2021, 7, 20), ClienteId = cliente1.Id, FechaRegistro = hoy, URLImagen = "/mascotas/mascota13.jpg" },

                    new Mascota { Nombre = "Chispa", Especie = "Hamster", Raza = "Roborovski", FechaNacimiento = new DateTime(2022, 12, 8), ClienteId = cliente2.Id, FechaRegistro = hoy, URLImagen = "/mascotas/mascota14.jpg" },

                    new Mascota { Nombre = "Loro", Especie = "Pájaro", Raza = "Loro Gris", FechaNacimiento = new DateTime(2018, 10, 15), ClienteId = cliente3.Id, FechaRegistro = hoy, URLImagen = "/mascotas/mascota15.jpg" }
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

            if (!_context.Medicamentos.Any())
            {
                var medicamentos = new[]
                     {
                        new Medicamento { Nombre = "Antibiotico A", Descripcion = "Para infecciones bacterianas", Dosis = "10 mg/kg", Stock = 8 },
                        new Medicamento { Nombre = "Analgesico B", Descripcion = "Para el dolor moderado", Dosis = "5 mg/kg", Stock = 12 },
                        new Medicamento { Nombre = "Antiparasitario C", Descripcion = "Para eliminar parasitos internos", Dosis = "2 mg/kg", Stock = 7 },
                        new Medicamento { Nombre = "Antiinflamatorio D", Descripcion = "Para inflamación y dolor", Dosis = "8 mg/kg", Stock = 10 },
                        new Medicamento { Nombre = "Vitamina E", Descripcion = "Suplemento antioxidante", Dosis = "1 mg/kg", Stock = 9 },
                        new Medicamento { Nombre = "Antihistaminico E", Descripcion = "Para alergias y reacciones alérgicas", Dosis = "0.5 mg/kg", Stock = 6 },
                        new Medicamento { Nombre = "Antiparasitario F", Descripcion = "Para eliminar parásitos externos", Dosis = "3 mg/kg", Stock = 15 },
                        new Medicamento { Nombre = "Sedante G", Descripcion = "Para sedación leve", Dosis = "1.5 mg/kg", Stock = 5 },
                        new Medicamento { Nombre = "Antiácido H", Descripcion = "Para tratar la acidez estomacal", Dosis = "2 mg/kg", Stock = 8 },
                        new Medicamento { Nombre = "Antibiótico I", Descripcion = "Para infecciones respiratorias", Dosis = "15 mg/kg", Stock = 11 },
                        new Medicamento { Nombre = "Antivirales J", Descripcion = "Para enfermedades virales", Dosis = "5 mg/kg", Stock = 7 },
                        new Medicamento { Nombre = "Antidiarreico K", Descripcion = "Para tratar la diarrea", Dosis = "3 mg/kg", Stock = 9 },
                        new Medicamento { Nombre = "Antihelmíntico L", Descripcion = "Para eliminar lombrices intestinales", Dosis = "1 mg/kg", Stock = 13 },
                        new Medicamento { Nombre = "Antiinflamatorio M", Descripcion = "Para tratar inflamación severa", Dosis = "12 mg/kg", Stock = 6 },
                        new Medicamento { Nombre = "Inmunoestimulante N", Descripcion = "Para estimular el sistema inmunológico", Dosis = "1 mg/kg", Stock = 7 },
                        new Medicamento { Nombre = "Antidepresivo O", Descripcion = "Para tratar la ansiedad y depresión", Dosis = "0.5 mg/kg", Stock = 3 },
                        new Medicamento { Nombre = "Corticosteroides P", Descripcion = "Para reducir la inflamación severa", Dosis = "0.25 mg/kg", Stock = 9 },
                        new Medicamento { Nombre = "Anticoagulante Q", Descripcion = "Para prevenir la formación de coágulos sanguíneos", Dosis = "0.1 mg/kg", Stock = 2 }, 
                        new Medicamento { Nombre = "Suplemento R", Descripcion = "Suplemento para huesos y articulaciones", Dosis = "2 mg/kg", Stock = 12 },
                        new Medicamento { Nombre = "Antifúngico S", Descripcion = "Para tratar infecciones por hongos", Dosis = "3 mg/kg", Stock = 2 } 
                    };


                _context.Medicamentos.AddRange(medicamentos);
                await _context.SaveChangesAsync();
                Console.WriteLine("Medicamentos creados.");
            }
            else
            {
                Console.WriteLine("Medicamentos ya existen.");
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