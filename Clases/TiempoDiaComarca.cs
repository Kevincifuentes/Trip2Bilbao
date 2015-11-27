using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class TiempoDiaComarca
    {
        public TiempoDiaComarca()
        {
            
        }

        public TiempoDiaComarca(DateTime diaPrediccion, string vientoEs, string vientoEu, string tiempoEs, string tiempoEu, string temperaturaEs, string temperaturaEu, string descripcionEs, string descripcionEu)
        {
            this.diaPrediccion = diaPrediccion;
            vientoES = vientoEs;
            vientoEU = vientoEu;
            tiempoES = tiempoEs;
            tiempoEU = tiempoEu;
            temperaturaES = temperaturaEs;
            temperaturaEU = temperaturaEu;
            descripcionES = descripcionEs;
            descripcionEU = descripcionEu;
        }

        public DateTime diaPrediccion { get; set; }
        public string vientoES { get; set; }
        public string vientoEU { get; set; }
        public string tiempoES { get; set; }
        public string tiempoEU { get; set; }
        public string temperaturaES { get; set; }
        public string temperaturaEU { get; set; }
        public string descripcionES { get; set; }
        public string descripcionEU { get; set; }

        public override string ToString()
        {
            string respuesta = "\t Día de predicción: " + diaPrediccion + " Viento: "+vientoES+ " Viento EU: "+vientoEU+"\n";
            respuesta = respuesta + "Tiempo: " + tiempoES + " Tiempo EU: " + tiempoEU + "\n";
            respuesta = respuesta + "Temperatura: " + temperaturaES + " Temperatura EU: " + temperaturaEU + "\n";
            respuesta = respuesta + "Descripcion: " + descripcionES + " Descripcion EU: " + descripcionEU + "\n";
            return respuesta;
        }
    }
}
