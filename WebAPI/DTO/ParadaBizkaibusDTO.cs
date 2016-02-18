using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class ParadaBizkaibusDTO
    {
        public ParadaBizkaibusDTO()
        {
            
        }

        public ParadaBizkaibusDTO(string nombreParada, double latitud, double longitud, int codigoPostal)
        {
            this.nombreParada = nombreParada;
            this.latitud = latitud;
            this.longitud = longitud;
            this.codigoPostal = codigoPostal;
        }

        public string nombreParada { get; set; }
        public double latitud { get; set; }
        public double longitud { get; set; }
        public double codigoPostal { get; set; }

    }
}
