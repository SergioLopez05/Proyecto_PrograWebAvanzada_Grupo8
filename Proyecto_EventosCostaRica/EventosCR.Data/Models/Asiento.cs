using System;
using System.Collections.Generic;

namespace EventosCR.Data.Models;

public partial class Asiento
{
    public int Id { get; set; }

    public int EventoId { get; set; }

    public int Fila { get; set; }

    public int Numero { get; set; }

    public bool? Disponible { get; set; }

    public virtual Boleto? Boleto { get; set; }

    public virtual Evento Evento { get; set; } = null!;
}
