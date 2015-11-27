using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class LineaBusTiempo
    {
        public LineaBusTiempo()
        {
            
        }

        public LineaBusTiempo(string codigoLinea, string descripcionLinea, int tiempoEspera)
        {
            this.codigoLinea = codigoLinea;
            this.descripcionLinea = descripcionLinea;
            this.tiempoEspera = tiempoEspera;
        }

        public string codigoLinea { get; set; }
        public string descripcionLinea { get; set; }
        public int tiempoEspera { get; set; }

    }
}
