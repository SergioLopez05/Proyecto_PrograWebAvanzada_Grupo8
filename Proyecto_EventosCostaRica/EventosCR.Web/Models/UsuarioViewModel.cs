using System;
using System.ComponentModel.DataAnnotations;

namespace EventosCR.Web.Models
{
    public class UsuarioViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Apellidos son obligatorios")]
        public string Apellidos { get; set; }

        [Phone]
        public string Telefono { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Rol { get; set; } = "Usuario";

        public bool Activo { get; set; } = true;

        public DateTime? FechaCreacion { get; set; }
    }
}


