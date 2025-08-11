namespace EventosCR.Web.Models { 

public class CompraBoletosViewModel
{
    public int EventoId { get; set; }
    public string EventoNombre { get; set; }
    public List<AsientoViewModel> Asientos { get; set; } = new List<AsientoViewModel>();
}
}