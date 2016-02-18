using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class PuntoBiciDTO
    {
        public PuntoBiciDTO()
        {
            
        }

        public PuntoBiciDTO(string nombrePunto, double latitud, double longitud, string estado, int capacidad, int codigoPostal)
        {
            this.nombrePunto = nombrePunto;
            this.estado = estado;
            this.capacidad = capacidad;
            this.latitud = latitud;
            this.longitud = longitud;
            this.codigoPostal = codigoPostal;
        }

        public string nombrePunto { get; set; }
        public string estado { get; set; }
        public int capacidad { get; set; }
        public double latitud { get; set; }
        public double longitud { get; set; }
        public double codigoPostal { get; set; }

    }
}
