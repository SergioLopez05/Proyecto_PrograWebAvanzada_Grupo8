using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventosCR.BLL.DTOs
{
    public class BoletoDto
    {
        public int Id { get; set; }
        public string NombreEvento { get; set; }
        public DateTime FechaEvento { get; set; }
        public int Fila { get; set; }
        public int Numero { get; set; }
        public decimal Precio { get; set; }
    }

}
