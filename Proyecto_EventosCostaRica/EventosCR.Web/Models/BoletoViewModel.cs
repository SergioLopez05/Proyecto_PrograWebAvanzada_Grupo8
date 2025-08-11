namespace EventosCR.Web.Models
{ 
public class BoletoViewModel
{
    public int Id { get; set; }
    public string NombreEvento { get; set; }
    public DateTime FechaEvento { get; set; }
    public int Fila { get; set; }
    public int Numero { get; set; }
    public decimal Precio { get; set; }
}
}