using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class PuntoBici
    {
        public PuntoBici()
        {
            
        }

        public PuntoBici(int id, string nombre, string estado, Coordenadas localizacion, int anclajesLibres, int anclajesAveriados, int anclajesUsados, int bicisLibres, int bicisAveriadas)
        {
            this.id = id;
            this.nombre = nombre;
            this.estado = estado;
            this.localizacion = localizacion;
            this.anclajesLibres = anclajesLibres;
            this.anclajesAveriados = anclajesAveriados;
            this.anclajesUsados = anclajesUsados;
            this.bicisLibres = bicisLibres;
            this.bicisAveriadas = bicisAveriadas;
        }

        public int id { get; set; }
        public string nombre { get; set; }
        public string estado { get; set; }
        public Coordenadas localizacion { get; set; }
        public int anclajesLibres { get; set; }
        public int anclajesAveriados { get; set; }
        public int anclajesUsados { get; set; }
        public int bicisLibres { get; set; }
        public int bicisAveriadas { get; set; }

        public override string ToString()
        {
            string respuesta = "ID: " + id + " " + nombre + " : " + "Localizacion " + localizacion.latitud+" , "+localizacion.longitud + " Estado: " + estado + "\n";
            respuesta = respuesta + "Anclajes Libres: " + anclajesLibres + " Anclajes Averiados: " + anclajesAveriados +
                        " Anclajes Usados: " + anclajesUsados + "\n";
            respuesta = respuesta + "Bicis Libres: " + bicisLibres + " Bicis Averiadas: " + bicisAveriadas +"\n";

            return respuesta;
        }
    }
}
