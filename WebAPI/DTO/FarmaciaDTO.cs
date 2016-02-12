using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class FarmaciaDTO
    {

        public FarmaciaDTO(string nombreFarmacia, int codigoPostal, string provincia, string direccionAbreviada, string ciudad, string urlInfo, double latitud, double longitud, long telefono)
        {
            this.nombreFarmacia = nombreFarmacia;
            this.codigoPostal = codigoPostal;
            this.provincia = provincia;
            this.direccionAbreviada = direccionAbreviada;
            this.ciudad = ciudad;
            this.urlInfo = urlInfo;
            this.latitud = latitud;
            this.longitud = longitud;
            this.telefono = telefono;
        }

        public string nombreFarmacia { get; set; }
        public int codigoPostal { get; set; }
        public string provincia { get; set; }
        public string direccionAbreviada { get; set; }
        public string ciudad { get; set; }
        public string urlInfo { get; set; }
        public double latitud { get; set; }
        public double longitud { get; set; }
        public long telefono { get; set; }
    }
}
