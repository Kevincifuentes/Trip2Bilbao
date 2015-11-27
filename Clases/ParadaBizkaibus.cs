using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class ParadaBizkaibus
    {
        public ParadaBizkaibus()
        {
            
        }

        public ParadaBizkaibus(int id, int codigo, string nombreParada, string descripcion, Coordenadas localizacion, string urlParada, string tipoLocalizacion, string idParadaPadre)
        {
            this.id = id;
            this.codigo = codigo;
            this.nombreParada = nombreParada;
            this.descripcion = descripcion;
            this.localizacion = localizacion;
            this.urlParada = urlParada;
            this.tipoLocalizacion = tipoLocalizacion;
            this.idParadaPadre = idParadaPadre;
        }

        public int id { get; set; }
        public int codigo { get; set; }
        public string nombreParada { get; set; }
        public string descripcion { get; set; }
        public Coordenadas localizacion { get; set; }
        public string urlParada { get; set; }
        public string tipoLocalizacion { get; set; }
        public string idParadaPadre { get; set; }

        public override string ToString()
        {
            string respuesta = "Parada " + nombreParada + " : ID " + id + " Descripción " + descripcion + " Lugar: " + localizacion.longitud + " , " + localizacion.latitud+"\n";
            respuesta = respuesta +  "Url Parada " + urlParada + " Tipo Localizacion " + tipoLocalizacion + " ID parada padre " + idParadaPadre +"\n";
            return respuesta;
        }
    }
}
