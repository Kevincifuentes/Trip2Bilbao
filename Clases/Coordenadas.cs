using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class Coordenadas
    {
        public Coordenadas()
        {

        }

        public Coordenadas(double latitud, double longitud)
        {
            this.latitud = latitud;
            this.longitud = longitud;
        }

        public double latitud { get; set; }

        public double longitud { get; set; }

    }
}
