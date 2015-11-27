using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class TiempoDia
    {
        public TiempoDia()
        {
            
        }

        public TiempoDia(DateTime fechaDeLaPrediccion, string vientoEs, string vientoEu, string tiempoEs, string tiempoEu, string temperaturaEs, string temperaturaEu)
        {
            this.fechaDeLaPrediccion = fechaDeLaPrediccion;
            vientoES = vientoEs;
            vientoEU = vientoEu;
            tiempoES = tiempoEs;
            tiempoEU = tiempoEu;
            temperaturaES = temperaturaEs;
            temperaturaEU = temperaturaEu;
        }

        public DateTime fechaDeLaPrediccion { get; set; }
        public string vientoES { get; set; }
        public string vientoEU { get; set; }
        public string tiempoES { get; set; }
        public string tiempoEU { get; set; }
        public string temperaturaES { get; set; }
        public string temperaturaEU { get; set; }

        public override string ToString()
        {
            string respuesta = "\tFecha de predicción: " + fechaDeLaPrediccion + "\n";
            respuesta = respuesta + "\tVientoES/VientoEU: " + vientoES + " / " + vientoEU + "\n";
            respuesta = respuesta + "\tTiempoES/TiempoEU: " + tiempoES + " / " + tiempoEU + "\n";
            respuesta = respuesta + "\tTemperaturaES/TemperaturaEU: " + temperaturaES + " / " + temperaturaEU + "\n";
            return respuesta;
        }
    }
}
