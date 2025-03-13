using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Betterinarie_Back.Application.Dtos.Implementation;
using Betterinarie_Back.Application.Interfaces.Implementation;
using Betterinarie_Back.Core.Interfaces.Data;

namespace Veterinaria.API.Controllers.Implementation
{
    [Route("api/[controller]")]
    [ApiController]
    public class MascotasController : ControllerBase
    {
        private readonly IMascotaService _mascotaService;

        public MascotasController(IMascotaService mascotaService)
        {
            _mascotaService = mascotaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var mascotas = await _mascotaService.GetAllMascotas();
            return Ok(mascotas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var mascota = await _mascotaService.GetMascotaById(id);
            if (mascota == null) return NotFound();
            return Ok(mascota);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] MascotaDto mascotaDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var mascota = await _mascotaService.CreateMascota(mascotaDto);
            return CreatedAtAction(nameof(GetById), new { id = mascota.Id }, mascota);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] MascotaDto mascotaDto)
        {
            if (id != mascotaDto.Id) return BadRequest("El ID de la mascota no coincide");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _mascotaService.UpdateMascota(mascotaDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mascotaService.DeleteMascota(id);
            return NoContent();
        }

        [HttpGet("dueño/{clienteId}")]
        public async Task<IActionResult> GetByCliente(int clienteId)
        {
            var mascotas = await _mascotaService.GetMascotasByCliente(clienteId);
            return Ok(mascotas);
        }
    }
}