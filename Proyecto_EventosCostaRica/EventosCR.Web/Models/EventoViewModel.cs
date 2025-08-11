namespace EventosCR.Web.Models
{
    public class EventoViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaEvento { get; set; }
        public string Lugar { get; set; }
        public string Banner { get; set; }
        public decimal PrecioEntrada { get; set; }
        public bool Vendido { get; set; }
    }
}