using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class TiempoComarca
    {
        public TiempoComarca()
        {
            
        }

        public TiempoComarca(DateTime diaActualizacion, string nombreComarcaEs, string nombreComarcaEu, List<TiempoDiaComarca> tiempoComarcaDia)
        {
            this.diaActualizacion = diaActualizacion;
            this.nombreComarcaES = nombreComarcaEs;
            nombreComarcaEU = nombreComarcaEu;
            this.tiempoComarcaDia = tiempoComarcaDia;
        }

        public DateTime diaActualizacion { get; set; }
        public string nombreComarcaES { get; set; }
        public string nombreComarcaEU { get; set; }
        public List<TiempoDiaComarca> tiempoComarcaDia { get; set; }

        public override string ToString()
        {
            string respuesta = "Día de actualización: " + diaActualizacion + " Nombre Comarca ES: " + nombreComarcaES +
                               " Nombre Comarca EU: " + nombreComarcaEU + "\n";
            foreach (TiempoDiaComarca temp in tiempoComarcaDia)
            {
                respuesta = respuesta + temp.ToString() + "\n";
            }
            return respuesta;
        }
    }
}
