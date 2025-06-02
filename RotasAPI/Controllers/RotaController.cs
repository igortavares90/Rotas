using Microsoft.AspNetCore.Mvc;
using Rotas.Domain.Models;
using Rotas.Service.Services;

namespace RotasAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RotaController : ControllerBase
    {
        private readonly RotaService _rotaService;

        public RotaController(RotaService rotaService)
        {
            _rotaService = rotaService;
        }

        [HttpGet("BuscarTodasAsRotas")]
        public async Task<IActionResult> BuscarTodasAsRotas()
        {
            var rotas = await _rotaService.GetAllRotasAsync();
            return Ok(rotas);
        }

        [HttpGet("BuscarPorId/{id}")]
        public async Task<ActionResult<Rota>> BuscarPorId(int id)
        {
            var rota = await _rotaService.GetByIdAsync(id);
            if (rota == null)
                return NotFound();
            return Ok(rota);
        }

        [HttpPost("CriarRota")]
        public async Task<ActionResult<Rota>> CriarRota(Rota rota)
        {
            var id = await _rotaService.AddAsync(rota);
            rota.Id = id;
            return CreatedAtAction(nameof(CriarRota), new { id = rota.Id }, rota);
        }

        [HttpPut("AtualizarRota/{id}")]
        public async Task<IActionResult> AtualizarRota(int id, Rota rota)
        {
            if (id != rota.Id)
                return BadRequest("ID da rota não confere com o parâmetro.");

            var updated = await _rotaService.UpdateAsync(rota);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("ApagarRota/{id}")]
        public async Task<IActionResult> ApagarRota(int id)
        {
            var deleted = await _rotaService.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }

        [HttpGet("MelhorRota")]
        public async Task<ActionResult> MelhorRota([FromQuery] string origem, [FromQuery] string destino)
        {
            var (caminho, custo) = await _rotaService.CalcularMelhorRotaAsync(origem.ToUpper(), destino.ToUpper());

            if (custo < 0 || caminho.Count == 0)
                return NotFound("Rota não encontrada.");

            var rotaFormatada = string.Join(" - ", caminho);
            return Ok($"{rotaFormatada} ao custo de ${custo}");
        }
    }
}
