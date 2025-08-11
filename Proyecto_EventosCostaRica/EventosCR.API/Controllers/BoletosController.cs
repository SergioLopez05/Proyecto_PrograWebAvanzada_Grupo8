using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using EventosCR.BLL.DTOs;
using EventosCR.Data.Context;
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
        public async Task<IActionResult> GetBoletosPorUsuario(int usuarioId)
        {
            var boletos = await _context.Boletos
                .Include(b => b.Asiento)
                .ThenInclude(a => a.Evento)
                .Where(b => b.UsuarioId == usuarioId)
                .ToListAsync();

            var boletosDto = boletos.Select(b => new BoletoDto
            {
                Id = b.Id,
                NombreEvento = b.Asiento.Evento.Nombre,
                FechaEvento = b.Asiento.Evento.FechaEvento,
                Fila = b.Asiento.Fila,
                Numero = b.Asiento.Numero,
                Precio = b.Precio
            }).ToList();

            return Ok(boletosDto);
        }

        [HttpPost("comprar")]
        public async Task<IActionResult> ComprarBoleto(int usuarioId, int asientoId)
        {
            var asiento = await _context.Asientos
                .Include(a => a.Evento)
                .FirstOrDefaultAsync(a => a.Id == asientoId);

            if (asiento == null || asiento.Disponible == false)
                return BadRequest("Asiento no disponible.");

            var boleto = new Boleto
            {
                UsuarioId = usuarioId,
                EventoId = asiento.EventoId,  // Se obtiene del asiento
                AsientoId = asientoId,
                Precio = asiento.Evento.PrecioEntrada
            };

            _context.Boletos.Add(boleto);

            asiento.Disponible = false;

            await _context.SaveChangesAsync();

            var boletoDto = new BoletoDto
            {
                Id = boleto.Id,
                NombreEvento = asiento.Evento.Nombre,
                FechaEvento = asiento.Evento.FechaEvento,
                Fila = asiento.Fila,
                Numero = asiento.Numero,
                Precio = boleto.Precio
            };

            return Ok(boletoDto);
        }



    }
}
