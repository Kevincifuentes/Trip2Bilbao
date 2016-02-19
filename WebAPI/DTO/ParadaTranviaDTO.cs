using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class ParadaTranviaDTO
    {
        public ParadaTranviaDTO()
        {
            
        }

        public ParadaTranviaDTO(string nombreParada, double latitud, double longitud, int codigoPostal, string descripcion)
        {
            this.nombreParada = nombreParada;
            this.descripcion = descripcion;
            this.latitud = latitud;
            this.longitud = longitud;
            this.codigoPostal = codigoPostal;
        }

        public string nombreParada { get; set; }
        public double latitud { get; set; }
        public double longitud { get; set; }
        public string descripcion { get; set; }
        public double codigoPostal { get; set; }

    }
}
