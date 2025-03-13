using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Betterinarie_Back.Application.Dtos.Implementation;
using Betterinarie_Back.Application.Dtos.Security;
using Betterinarie_Back.Application.Interfaces.Implementation;

namespace Betterinarie_Back.Controllers.Implementation
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var usuarios = await _usuarioService.GetAllUsuarios();
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var usuario = await _usuarioService.GetUsuarioById(id);
            if (usuario == null) return NotFound();
            return Ok(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var usuario = await _usuarioService.CreateUsuario(registerDto);
            return CreatedAtAction(nameof(GetById), new { id = usuario.Id }, usuario);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UsuarioDto usuarioDto)
        {
            if (id != usuarioDto.Id) return BadRequest("El ID del usuario no coincide");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _usuarioService.UpdateUsuario(usuarioDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _usuarioService.DeleteUsuario(id);
            return NoContent();
        }

        [HttpPost("{usuarioId}/roles/{rolId}")]
        public async Task<IActionResult> AssignRol(int usuarioId, int rolId)
        {
            await _usuarioService.AssignRolToUsuario(usuarioId, rolId);
            return NoContent();
        }


        [HttpGet("{veterinarioId}/consultas")]
        public async Task<IActionResult> GetConsultas(int veterinarioId)
        {
            var consultas = await _usuarioService.GetConsultasByVeterinario(veterinarioId);
            return Ok(consultas);
        }
    }
}
