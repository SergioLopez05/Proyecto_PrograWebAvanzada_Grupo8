using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventosCR.Data.Models;

namespace EventosCR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsientosController : ControllerBase
    {
        private readonly EventosCostaRicaDbContext _context;

        public AsientosController(EventosCostaRicaDbContext context)
        {
            _context = context;
        }

        [HttpGet("evento/{eventoId}")]
        public async Task<ActionResult<IEnumerable<Asiento>>> GetAsientosPorEvento(int eventoId)
        {
            return await _context.Asientos
                .Where(a => a.EventoId == eventoId)
                .OrderBy(a => a.Fila)
                .ThenBy(a => a.Numero)
                .ToListAsync();
        }

        [HttpGet("disponibles/{eventoId}")]
        public async Task<ActionResult<IEnumerable<Asiento>>> GetAsientosDisponibles(int eventoId)
        {
            return await _context.Asientos
                .Where(a => a.EventoId == eventoId && a.Disponible == true)
                .OrderBy(a => a.Fila)
                .ThenBy(a => a.Numero)
                .ToListAsync();
        }
    }
}
