using Betterinarie_Back.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace Betterinarie_Back.Controllers.Data
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly SeedData _seeder;

        public SeedController(SeedData seeder)
        {
            _seeder = seeder;
        }

        [HttpPost]
        public async Task<IActionResult> SeedDatabase()
        {
            try
            {
                await _seeder.ExecuteSeed();
                return Ok("La base de datos se ha sembrado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al sembrar la base de datos: {ex.Message}");
            }
        }
    }
}
