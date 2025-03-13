using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Betterinarie_Back.Application.Dtos.Implementation;
using Betterinarie_Back.Application.Interfaces.Implementation;

namespace Betterinarie_Back.API.Controllers.Implementation
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultasController : ControllerBase
    {
        private readonly IConsultaService _consultaService;

        public ConsultasController(IConsultaService consultaService)
        {
            _consultaService = consultaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var consultas = await _consultaService.GetAllConsultas();
            return Ok(consultas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var consulta = await _consultaService.GetConsultaById(id);
            if (consulta == null) return NotFound();
            return Ok(consulta);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ConsultaDto consultaDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var consulta = await _consultaService.CreateConsulta(consultaDto);
            return CreatedAtAction(nameof(GetById), new { id = consulta.Id }, consulta);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ConsultaDto consultaDto)
        {
            if (id != consultaDto.Id) return BadRequest("El ID de la consulta no coincide");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _consultaService.UpdateConsulta(consultaDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _consultaService.DeleteConsulta(id);
            return NoContent();
        }

        [HttpPost("{consultaId}/medicamentos/{medicamentoId}")]
        public async Task<IActionResult> AssignMedicamento(int consultaId, int medicamentoId)
        {
            await _consultaService.AssignMedicamentoToConsulta(consultaId, medicamentoId);
            return NoContent();
        }

        [HttpGet("veterinario/{veterinarioId}")]
        public async Task<IActionResult> GetByVeterinario(int veterinarioId)
        {
            var consultas = await _consultaService.GetConsultasByVeterinario(veterinarioId);
            return Ok(consultas);
        }


        [HttpGet("mascota/{mascotaId}")]
        public async Task<IActionResult> GetByMascota(int mascotaId)
        {
            var consultas = await _consultaService.GetConsultasByMascota(mascotaId);
            return Ok(consultas);
        }
    }
}
