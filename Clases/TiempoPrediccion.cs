using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class TiempoPrediccion
    {
        public TiempoPrediccion()
        {
            
        }

        public TiempoPrediccion(DateTime fechaFinPrediccion, DateTime fechaInicioPrediccion, DateTime fechaRealizacionPrediccion, List<TiempoDia> predicciones, string fechaObtencion)
        {
            this.fechaFinPrediccion = fechaFinPrediccion;
            this.fechaInicioPrediccion = fechaInicioPrediccion;
            this.fechaRealizacionPrediccion = fechaRealizacionPrediccion;
            this.predicciones = predicciones;
            this.fechaObtencion = fechaObtencion;
        }

        public DateTime fechaFinPrediccion { get; set; }
        public DateTime fechaInicioPrediccion { get; set; }
        public DateTime fechaRealizacionPrediccion { get; set; }
        public List<TiempoDia> predicciones { get; set; }
        public string fechaObtencion { get; set; }

        public override string ToString()
        {
            string respuesta = "Fecha Inicio Prediccion /Final: " + fechaInicioPrediccion + "/" + fechaFinPrediccion +
                               " Fecha Realización: " + fechaRealizacionPrediccion +" Fecha Obtencion: "+fechaObtencion + "\n";
            foreach (TiempoDia pred in predicciones)
            {
                respuesta = respuesta + pred.ToString() + "\n";
            }
            return respuesta;
        }
    }
}
