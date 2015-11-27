using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class ParadaMetro
    {
        public ParadaMetro()
        {
            
        }

        public ParadaMetro(string id, string codigo, string nombreParada, Coordenadas localizacion, string tipoLocalizacion, string idParadaPadre)
        {
            this.id = id;
            this.codigo = codigo;
            this.nombreParada = nombreParada;
            this.localizacion = localizacion;
            this.tipoLocalizacion = tipoLocalizacion;
            this.idParadaPadre = idParadaPadre;
        }

        public string id { get; set; }
        public string codigo { get; set; }
        public string nombreParada { get; set; }
        public Coordenadas localizacion { get; set; }
        public string tipoLocalizacion { get; set; }
        public string idParadaPadre { get; set; }

        public override string ToString()
        {
            string respuesta = "Parada " + nombreParada + " : ID " + id  + " Lugar: " + localizacion.longitud + " , " + localizacion.latitud + "\n";
            respuesta = respuesta  + " Tipo Localizacion " + tipoLocalizacion + " ID parada padre " + idParadaPadre + "\n";
            return respuesta;
        }
    }
}
