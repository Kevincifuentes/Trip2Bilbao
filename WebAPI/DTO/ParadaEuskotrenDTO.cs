using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class ParadaEuskotrenDTO
    {
        public ParadaEuskotrenDTO()
        {
            
        }

        public ParadaEuskotrenDTO( string nombreParada, double latitud, double longitud, int codigoPostal, string codigoParada)
        {
            this.nombreParada = nombreParada;
            this.codigoParada = codigoParada;
            this.latitud = latitud;
            this.longitud = longitud;
            this.codigoPostal = codigoPostal;
        }

        public string nombreParada { get; set; }
        public double latitud { get; set; }
        public double longitud { get; set; }
        public string codigoParada { get; set; }
        public double codigoPostal { get; set; }

    }
}
