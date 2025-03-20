using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Betterinarie_Back.Application.Dtos.Implementation;
using Betterinarie_Back.Application.Interfaces.Implementation;
using Microsoft.AspNetCore.Authorization;

namespace Betterinarie_Back.API.Controllers.Implementation
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _clienteService;
        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var dueños = await _clienteService.GetAllClientes();
            return Ok(dueños);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var dueño = await _clienteService.GetClienteById(id);
            if (dueño == null) return NotFound();
            return Ok(dueño);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ClienteDto dueñoDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var dueño = await _clienteService.CreateCliente(dueñoDto);
            return CreatedAtAction(nameof(GetById), new { id = dueño.Id }, dueño);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ClienteUpdateDto clienteDto)
        {
            if (id != clienteDto.Id) return BadRequest("El ID del cliente no coincide");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _clienteService.UpdateCliente(clienteDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _clienteService.DeleteCliente(id);
            return NoContent();
        }
    }
}
