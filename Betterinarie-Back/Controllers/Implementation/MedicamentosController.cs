using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Betterinarie_Back.Application.Dtos.Implementation;
using Betterinarie_Back.Application.Interfaces.Implementation;
using Microsoft.AspNetCore.Authorization;

namespace Betterinarie_Back.API.Controllers.Implementation
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Veterinario,Administrador")]
    public class MedicamentosController : ControllerBase
    {
        private readonly IMedicamentoService _medicamentoService;

        public MedicamentosController(IMedicamentoService medicamentoService)
        {
            _medicamentoService = medicamentoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var medicamentos = await _medicamentoService.GetAllMedicamentos();
            return Ok(medicamentos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var medicamento = await _medicamentoService.GetMedicamentoById(id);
            if (medicamento == null) return NotFound();
            return Ok(medicamento);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MedicamentoDto medicamentoDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var medicamento = await _medicamentoService.CreateMedicamento(medicamentoDto);
            return CreatedAtAction(nameof(GetById), new { id = medicamento.Id }, medicamento);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MedicamentoDto medicamentoDto)
        {
            if (id != medicamentoDto.Id) return BadRequest("El ID del medicamento no coincide");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _medicamentoService.UpdateMedicamento(medicamentoDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _medicamentoService.DeleteMedicamento(id);
            return NoContent();
        }
    }
}
