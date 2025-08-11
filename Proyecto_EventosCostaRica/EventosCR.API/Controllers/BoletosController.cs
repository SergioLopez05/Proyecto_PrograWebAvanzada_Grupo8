using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventosCR.Data.Models;

namespace EventosCR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoletosController : ControllerBase
    {
        private readonly EventosCostaRicaDbContext _context;

        public BoletosController(EventosCostaRicaDbContext context)
        {
            _context = context;
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<IEnumerable<Boleto>>> GetBoletosPorUsuario(int usuarioId)
        {
            return await _context.Boletos
                .Include(b => b.Evento)
                .Include(b => b.Asiento)
                .Where(b => b.UsuarioId == usuarioId)
                .ToListAsync();
        }

        [HttpPost("comprar")]
        public async Task<IActionResult> ComprarBoleto(int usuarioId, int eventoId, int asientoId)
        {
            var asiento = await _context.Asientos.FindAsync(asientoId);
            if (asiento == null || asiento.Disponible == false) return BadRequest("Asiento no disponible.");

            var evento = await _context.Eventos.FindAsync(eventoId);
            if (evento == null) return NotFound("Evento no encontrado.");

            asiento.Disponible = false;

            var boleto = new Boleto
            {
                UsuarioId = usuarioId,
                EventoId = eventoId,
                AsientoId = asientoId,
                Precio = evento.PrecioEntrada
            };

            _context.Boletos.Add(boleto);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Boleto comprado con éxito", boleto });
        }
    }
}
