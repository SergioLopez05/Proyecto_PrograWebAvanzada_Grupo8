using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventosCR.Data.Models;
using Microsoft.AspNetCore.Authorization;
using EventosCR.Data.Context;

namespace EventosCR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventosController : ControllerBase
    {
        private readonly EventosCostaRicaDbContext _context;

        public EventosController(EventosCostaRicaDbContext context)
        {
            _context = context;
        }


        // GET público
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Evento>>> GetEventos()
        {
            return await _context.Eventos
                                 .Where(e => e.Activo == true)
                                 .OrderBy(e => e.FechaEvento)
                                 .ToListAsync();
        }



        // GET detalle público
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Evento>> GetEvento(int id)
        {
            var evento = await _context.Eventos.FindAsync(id);
            if (evento == null) return NotFound();
            return evento;
        }

        // Crear - solo Admin
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<Evento>> CreateEvento(Evento evento)
        {
            _context.Eventos.Add(evento);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetEvento), new { id = evento.Id }, evento);
        }

        // Editar - solo Admin
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> UpdateEvento(int id, Evento evento)
        {
            if (id != evento.Id) return BadRequest();
            _context.Entry(evento).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Borrar - solo Admin
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteEvento(int id)
        {
            var evento = await _context.Eventos.FindAsync(id);
            if (evento == null) return NotFound();

            _context.Eventos.Remove(evento);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
