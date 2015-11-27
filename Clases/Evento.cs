using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
     public class Evento
     {
         public Evento()
         {
             
         }

        public Evento(int id, DateTime fechaInicio, DateTime fechaFin, string descripcion, Coordenadas localizacion)
        {
            this.id = id;
            this.fechaInicio = fechaInicio;
            this.fechaFin = fechaFin;
            this.descripcion = descripcion;
            this.localizacion = localizacion;
        }

        public int id { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public string descripcion { get; set; }
        public Coordenadas localizacion { get; set; }

        public override string ToString()
        {
            return "ID: " + id + " Fecha Inicio " + fechaInicio + " Fecha Fin " + fechaFin + " Descripcion: " +
                   descripcion + "Localizacion: " + localizacion.latitud + " / " + localizacion.longitud + " \n";
        }

    }
}
