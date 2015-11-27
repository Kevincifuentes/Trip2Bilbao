using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class ParadaTranvia
    {
        public ParadaTranvia()
        {
            
        }

        public ParadaTranvia(string nombre, string descripcion, Coordenadas localizacion)
        {
            this.nombre = nombre;
            this.descripcion = descripcion;
            this.localizacion = localizacion;
        }

        private string nombre { get; set; }
        private string descripcion { get; set; }
        private Coordenadas localizacion { get; set; }

        public override string ToString()
        {
            string respuesta = "Nombre: " + nombre + " Descripcion: " + descripcion + " Localizacion: " +
                               localizacion.latitud + "/" + localizacion.longitud+"\r\n";
            return respuesta;
        }
    }
}
