using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventosCR.Data.Models;
using EventosCR.Data.Context;

namespace EventosCR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly EventosCostaRicaDbContext _context;

        public UsuariosController(EventosCostaRicaDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();
            return usuario;
        }

        [HttpPost]
        public async Task<ActionResult<Usuario>> CreateUsuario(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuario);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUsuario(int id, Usuario usuario)
        {
            if (id != usuario.Id) return BadRequest();
            _context.Entry(usuario).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        //[HttpGet("email/{email}")]
        //public async Task<ActionResult<Usuario>> GetUsuarioByEmail(string email)
        //{
        //    if (string.IsNullOrWhiteSpace(email))
        //        return BadRequest("El email no puede estar vacío");

        //    var usuario = await _context.Usuarios
        //        .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());

        //    if (usuario == null)
        //        return NotFound($"No se encontró ningún usuario con el email: {email}");

        //    return usuario;
        //}

        [HttpGet("email/{email}")]
        public async Task<ActionResult<Usuario>> GetByEmail(string email)
        {
            var usuario = await _context.Usuarios
                .Where(u => u.Email == email)
                .Select(u => new Usuario
                {
                    Id = u.Id,
                    Nombre = u.Nombre,
                    Email = u.Email
                })
                .FirstOrDefaultAsync();

            if (usuario == null)
                return NotFound();

            return Ok(usuario);
        }


    }
}
