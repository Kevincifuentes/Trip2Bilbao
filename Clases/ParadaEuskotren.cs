using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class ParadaEuskotren
    {
        public ParadaEuskotren()
        {
            
        }

        public ParadaEuskotren(int id, string codigo, string nombre, string descripcion, Coordenadas localizacion, string idZona, string urlParada, string tipoLocalizacion)
        {
            this.id = id;
            this.codigo = codigo;
            this.nombre = nombre;
            this.descripcion = descripcion;
            this.localizacion = localizacion;
            this.idZona = idZona;
            this.urlParada = urlParada;
            this.tipoLocalizacion = tipoLocalizacion;
        }

        public int id { get; set; }
        public string codigo { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public Coordenadas localizacion { get; set; }
        public string idZona { get; set; }
        public string urlParada { get; set; }
        public string tipoLocalizacion { get; set; }


        public override string ToString()
        {
            string respuesta = "Parada y codigo: " + nombre +" "+codigo+ " ID: " + id +" ID zona: "+ idZona+ " Tipo / Lugar: " +tipoLocalizacion+ " / "+ localizacion.longitud + " , " + localizacion.latitud + "\n";
            respuesta = respuesta + " Tipo Localizacion " + tipoLocalizacion + "Url de parada " + urlParada + "\n";
            respuesta = respuesta + "Descripcion: "+descripcion+ "\n";
            return respuesta;
        }
    }
}
