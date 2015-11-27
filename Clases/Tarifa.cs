using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class Tarifa
    {
        public Tarifa()
        {
            
        }

        public Tarifa(string tipo, string descripcion, DateTime actualizacion)
        {
            this.tipo = tipo;
            this.descripcion = descripcion;
            this.actualizacion = actualizacion;
            this.zona = "Desconocida";
        }

        public string tipo { get; set; }
        public string descripcion { get; set; }
        public string zona { get; set; }
        public DateTime actualizacion { get; set; }

       
    }
}
