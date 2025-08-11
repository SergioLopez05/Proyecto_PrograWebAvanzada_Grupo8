using System;
using System.Collections.Generic;

namespace EventosCR.Data.Models;

public partial class Evento
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public DateTime FechaEvento { get; set; }

    public string Lugar { get; set; } = null!;

    public string? Banner { get; set; }

    public int? CapacidadTotal { get; set; }

    public decimal PrecioEntrada { get; set; }

    public bool? Vendido { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public bool? Activo { get; set; }

    public virtual ICollection<Asiento> Asientos { get; set; } = new List<Asiento>();

    public virtual ICollection<Boleto> Boletos { get; set; } = new List<Boleto>();
}
