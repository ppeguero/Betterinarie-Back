using AutoMapper;
using Betterinarie_Back.Application.Dtos.Implementation;
using Betterinarie_Back.Application.Dtos.Security;
using Betterinarie_Back.Application.Interfaces.Implementation;
using Betterinarie_Back.Application.Interfaces.Security;
using Betterinarie_Back.Core.Entities.Implementation;
using Betterinarie_Back.Core.Interfaces.Implementation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Betterinarie_Back.Application.Services.Implementation
{
    public class UsuarioService : IUsuarioService
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly RoleManager<Rol> _roleManager;
        private readonly IRepository<Mascota> _mascotaRepository;
        private readonly IRepository<Consulta> _consultaRepository;
        private readonly IMapper _mapper;
        private readonly IErrorLogService _errorLogService;

        public UsuarioService(
            UserManager<Usuario> userManager,
            RoleManager<Rol> roleManager,
            IRepository<Mascota> mascotaRepository,
            IRepository<Consulta> consultaRepository,
            IMapper mapper,
            IErrorLogService errorLogService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mascotaRepository = mascotaRepository;
            _consultaRepository = consultaRepository;
            _mapper = mapper;
            _errorLogService = errorLogService;
        }

        public async Task<UsuarioDto> GetUsuarioById(int id)
        {
            try
            {
                var usuario = await _userManager.Users
                    .Include(u => u.Consultas)
                    .FirstOrDefaultAsync(u => u.Id == id);
                if (usuario == null) return null;
                var roles = await _userManager.GetRolesAsync(usuario);
                var usuarioDto = _mapper.Map<UsuarioDto>(usuario);
                usuarioDto.Roles = roles.ToList();
                return usuarioDto;
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(
                    message: "Error al obtener usuario por ID",
                    stackTrace: ex.StackTrace,
                    userId: id.ToString()
                );
                throw;
            }
        }

        public async Task<IEnumerable<UsuarioDto>> GetAllUsuarios()
        {
            try
            {
                var usuarios = await _userManager.Users
                    .Include(u => u.Consultas)
                    .ToListAsync();
                var usuarioDtos = new List<UsuarioDto>();
                foreach (var usuario in usuarios)
                {
                    var roles = await _userManager.GetRolesAsync(usuario);
                    var usuarioDto = _mapper.Map<UsuarioDto>(usuario);
                    usuarioDto.Roles = roles.ToList();
                    usuarioDtos.Add(usuarioDto);
                }
                return usuarioDtos;
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(
                    message: "Error al obtener todos los usuarios",
                    stackTrace: ex.StackTrace,
                    userId: null
                );
                throw;
            }
        }

        public async Task<UsuarioDto> CreateUsuario(RegisterDto createDto)
        {
            try
            {
                var usuario = new Usuario
                {
                    UserName = createDto.Email, 
                    Email = createDto.Email,
                    Nombre = createDto.Nombre,
                    Apellido = createDto.Apellido
                };
                var result = await _userManager.CreateAsync(usuario, createDto.Password);
                if (!result.Succeeded)
                {
                    throw new Exception($"Error al crear usuario: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }

                if (createDto.RolId.HasValue)
                {
                    await AssignRolToUsuario(usuario.Id, createDto.RolId.Value);
                }
                var usuarioDto = _mapper.Map<UsuarioDto>(usuario);
                var roles = await _userManager.GetRolesAsync(usuario);
                usuarioDto.Roles = roles.ToList();

                return usuarioDto;
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(
                    message: "Error al crear usuario",
                    stackTrace: ex.StackTrace,
                    userId: null
                );
                throw;
            }
        }

        public async Task<UsuarioDto> UpdateUsuarioForAdmin(UsuarioEditDto editDto)
        {
            try
            {
                var usuario = await _userManager.FindByIdAsync(editDto.Id.ToString());
                if (usuario == null) throw new Exception("Usuario no encontrado");

                _mapper.Map(editDto, usuario);

                var result = await _userManager.UpdateAsync(usuario);
                if (!result.Succeeded)
                {
                    throw new Exception($"Error al actualizar usuario: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
                if (!string.IsNullOrEmpty(editDto.Password))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(usuario);
                    var passwordResult = await _userManager.ResetPasswordAsync(usuario, token, editDto.Password);
                    if (!passwordResult.Succeeded)
                    {
                        throw new Exception($"Error al actualizar la contraseña: {string.Join(", ", passwordResult.Errors.Select(e => e.Description))}");
                    }
                }

                if (editDto.RolId.HasValue) 
                {
                    var cRoles = await _userManager.GetRolesAsync(usuario);
                    if (cRoles.Any())
                    {
                        await _userManager.RemoveFromRolesAsync(usuario, cRoles);
                    }
                    await AssignRolToUsuario(usuario.Id, editDto.RolId.Value);
                }

                var usuarioDto = _mapper.Map<UsuarioDto>(usuario);
                var roles = await _userManager.GetRolesAsync(usuario);
                usuarioDto.Roles = roles.ToList();
                return usuarioDto;
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(
                message: "Error al actualizar usuario desde admin",
                stackTrace: ex.StackTrace,
                userId: editDto.Id.ToString()
                );
                throw;
            }
        }

        public async Task UpdateUsuario(UsuarioDto updateDto)
        {
            try
            {
                var usuario = await _userManager.FindByIdAsync(updateDto.Id.ToString());
                if (usuario == null) throw new Exception("Usuario no encontrado");

                usuario.Nombre = updateDto.Nombre;
                usuario.Apellido = updateDto.Apellido;
                usuario.Email = updateDto.Email;
                usuario.UserName = updateDto.Email;

                var result = await _userManager.UpdateAsync(usuario);
                if (!result.Succeeded)
                {
                    throw new Exception($"Error al actualizar usuario: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(
                    message: "Error al actualizar usuario",
                    stackTrace: ex.StackTrace,
                    userId: updateDto.Id.ToString()
                );
                throw;
            }
        }

        public async Task UpdatePassword(int usuarioId, string newPassword)
        {
            try
            {
                var usuario = await _userManager.FindByIdAsync(usuarioId.ToString());
                if (usuario == null) throw new Exception("Usuario no encontrado");

                var token = await _userManager.GeneratePasswordResetTokenAsync(usuario);
                var result = await _userManager.ResetPasswordAsync(usuario, token, newPassword);
                if (!result.Succeeded)
                {
                    throw new Exception($"Error al actualizar la contraseña: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(
                    message: "Error al actualizar la contraseña",
                    stackTrace: ex.StackTrace,
                    userId: usuarioId.ToString()
                );
                throw;
            }
        }

        public async Task DeleteUsuario(int id)
        {
            try
            {
                var usuario = await _userManager.FindByIdAsync(id.ToString());
                if (usuario == null) throw new Exception("Usuario no encontrado");

                var result = await _userManager.DeleteAsync(usuario);
                if (!result.Succeeded)
                {
                    throw new Exception($"Error al eliminar usuario: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(
                    message: "Error al eliminar usuario",
                    stackTrace: ex.StackTrace,
                    userId: id.ToString()
                );
                throw;
            }
        }

        public async Task AssignRolToUsuario(int usuarioId, int rolId)
        {
            try
            {
                var usuario = await _userManager.FindByIdAsync(usuarioId.ToString());
                if (usuario == null) throw new Exception("Usuario no encontrado");

                var rol = await _roleManager.FindByIdAsync(rolId.ToString());
                if (rol == null) throw new Exception("Rol no encontrado");

                if (!await _userManager.IsInRoleAsync(usuario, rol.Name))
                {
                    await _userManager.AddToRoleAsync(usuario, rol.Name);
                }
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(
                    message: "Error al asignar rol a usuario",
                    stackTrace: ex.StackTrace,
                    userId: usuarioId.ToString()
                );
                throw;
            }
        }

        public async Task<IEnumerable<MascotaDto>> GetMascotasByUsuario(int usuarioId)
        {
            try
            {
                var mascotas = await _mascotaRepository.GetAll();
                var mascotasUsuario = mascotas.Where(m => m.UsuarioId == usuarioId);
                return _mapper.Map<IEnumerable<MascotaDto>>(mascotasUsuario);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(
                    message: "Error al obtener mascotas por usuario",
                    stackTrace: ex.StackTrace,
                    userId: usuarioId.ToString()
                );
                throw;
            }
        }

        public async Task<IEnumerable<ConsultaDto>> GetConsultasByVeterinario(int veterinarioId)
        {
            try
            {
                var consultas = await _consultaRepository.GetAll();
                var consultasVeterinario = consultas.Where(c => c.VeterinarioId == veterinarioId);
                return _mapper.Map<IEnumerable<ConsultaDto>>(consultasVeterinario);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(
                    message: "Error al obtener consultas por veterinario",
                    stackTrace: ex.StackTrace,
                    userId: veterinarioId.ToString()
                );
                throw;
            }
        }
    }
}