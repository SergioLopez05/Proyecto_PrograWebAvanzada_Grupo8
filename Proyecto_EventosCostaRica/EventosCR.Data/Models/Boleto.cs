using System;
using System.Collections.Generic;

namespace EventosCR.Data.Models;

public partial class Boleto
{
    public int Id { get; set; }

    public int UsuarioId { get; set; }

    public int EventoId { get; set; }

    public int AsientoId { get; set; }

    public DateTime? FechaCompra { get; set; }

    public decimal Precio { get; set; }

    public virtual Asiento Asiento { get; set; } = null!;

    public virtual Evento Evento { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
