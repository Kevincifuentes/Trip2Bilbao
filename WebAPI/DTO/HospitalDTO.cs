using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class HospitalDTO
    {
        public HospitalDTO()
        {
            
        }

        public HospitalDTO(string nombreHospital, string direccionCompleta, double latitud, double longitud, string region, string ciudad, string web, long telefono)
        {
            this.nombreHospital = nombreHospital;
            this.direccionCompleta = direccionCompleta;
            this.region = region;
            this.ciudad = ciudad;
            this.latitud = latitud;
            this.longitud = longitud;
            this.web = web;
            this.telefono = telefono;
        }

        public string nombreHospital { get; set; }
        public string direccionCompleta { get; set; }
        public string region { get; set; }
        public string ciudad { get; set; }
        public double latitud { get; set; }
        public double longitud { get; set; }
        public string web { get; set; }
        public long telefono { get; set; }

    }
}
