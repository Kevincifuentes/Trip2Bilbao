using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class TiempoDiaCiudad
    {
        public TiempoDiaCiudad()
        {
            
        }
        public TiempoDiaCiudad(string nombreCiudad, int idCiudad, string descripcionEs, string descripcionEu, int maxima, int minima)
        {
            this.nombreCiudad = nombreCiudad;
            this.idCiudad = idCiudad;
            descripcionES = descripcionEs;
            descripcionEU = descripcionEu;
            this.maxima = maxima;
            this.minima = minima;
        }

        public string nombreCiudad { get; set; }
        public int idCiudad { get; set; }
        public string descripcionES { get; set; }
        public string descripcionEU { get; set; }
        public int maxima { get; set; }
        public int minima { get; set; }

        public override string ToString()
        {
            string respuesta = " Nombre Ciudad: " + nombreCiudad +
                               " ID Ciudad: " + idCiudad + "\n";
            respuesta = respuesta + "DescripciónES: " + descripcionES + "\n";
            respuesta = respuesta + "DescripciónEU: " + descripcionEU + "\n";
            respuesta = respuesta + " Max: "+maxima+" Min: "+minima+ "\n";
            return respuesta;
        }
    }
}
