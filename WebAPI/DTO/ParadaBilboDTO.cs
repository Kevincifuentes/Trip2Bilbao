using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class ParadaBilboDTO
    {
        public ParadaBilboDTO()
        {
            
        }

        public ParadaBilboDTO(string nombreParada, double latitud, double longitud, string abreviatura, int codigoPostal)
        {
            this.nombreParada = nombreParada;
            this.abreviatura = abreviatura;
            this.latitud = latitud;
            this.longitud = longitud;
            this.codigoPostal = codigoPostal;
        }

        public string nombreParada { get; set; }
        public string abreviatura { get; set; }
        public double latitud { get; set; }
        public double longitud { get; set; }
        public double codigoPostal { get; set; }

    }
}
