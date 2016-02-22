using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class IncidenciaDTO
    {

        public IncidenciaDTO()
        {
            
        }

        public IncidenciaDTO(string tipo, string descripcion, double latitud, double longitud, DateTime inicio, DateTime fin)
        {
            this.tipo = tipo;
            this.descripcion = descripcion;
            this.latitud = latitud;
            this.longitud = longitud;
            this.inicio = inicio;
            this.fin = fin;
        }

        public string tipo { get; set; }
        public string descripcion { get; set; }
        public double latitud { get; set; }
        public double longitud { get; set; }
        public DateTime inicio { get; set; }
        public DateTime fin { get; set; }

    }
}
