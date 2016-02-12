using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class CentroSaludDTO
    {

        public CentroSaludDTO()
        {
            
        }

        public CentroSaludDTO(string nombreCentro, string direccionCompleta, int codigoPostal, string provincia, string region, string horario, string ciudad, string urlAdicional, double latitud, double longitud, string web, long telefono)
        {
            this.nombreCentro = nombreCentro;
            this.direccionCompleta = direccionCompleta;
            this.codigoPostal = codigoPostal;
            this.provincia = provincia;
            this.region = region;
            this.horario = horario;
            this.ciudad = ciudad;
            this.urlAdicional = urlAdicional;
            this.latitud = latitud;
            this.longitud = longitud;
            this.web = web;
            this.telefono = telefono;
        }

        public string nombreCentro { get; set; }
        public string direccionCompleta { get; set; }
        public int codigoPostal { get; set; }
        public string provincia { get; set; }
        public string region { get; set; }
        public string horario { get; set; }
        public string ciudad { get; set; }
        public string urlAdicional { get; set; }
        public double latitud { get; set; }
        public double longitud { get; set; }
        public string web { get; set; }
        public long telefono { get; set; }

    }
}
