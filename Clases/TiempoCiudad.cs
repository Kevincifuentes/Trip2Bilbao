using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class TiempoCiudad
    {

        public TiempoCiudad()
        {
            
        }

        public TiempoCiudad(DateTime horaDeActualizacion, DateTime dia, string descripcionEs, string descripcionEu,
            string fechaEscrita, Dictionary<string, TiempoDiaCiudad> tiempoCiudades)
        {
            this.horaDeActualizacion = horaDeActualizacion;
            this.dia = dia;
            descripcionES = descripcionEs;
            descripcionEU = descripcionEu;
            this.fechaEscrita = fechaEscrita;
            this.tiempoCiudades = tiempoCiudades;
        }

        public DateTime horaDeActualizacion { get; set; }
        public DateTime dia { get; set; }
        public string descripcionES { get; set; }
        public string descripcionEU { get; set; }
        public string fechaEscrita { get; set; }
        public Dictionary<string, TiempoDiaCiudad> tiempoCiudades { get; set; }
        public override string ToString()
        {
            string respuesta = "Hora de Actualización: " + horaDeActualizacion + " Día: " + dia + "\n";
            respuesta = respuesta + "DescripcionES: " + descripcionES + "\n";
            respuesta = respuesta + "DescripcionEU: " + descripcionEU + "\n";
            respuesta = respuesta + "Fecha escrita: " + fechaEscrita + "\n";

            return respuesta;
        }
    }
}
