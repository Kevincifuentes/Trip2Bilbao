using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class ParadaBilbo
    {
        public ParadaBilbo()
        {
            
        }

        public ParadaBilbo(int id, int idParada, Coordenadas lugar, string nombre, string nombreCorto)
        {
            this.id = id;
            this.idParada = idParada;
            this.lugar = lugar;
            this.nombre = nombre;
            this.nombreCorto = nombreCorto;
            this.lineasYTiempo = new List<KeyValuePair<string, LineaBusTiempo>>();
        }


        public int id { get; set; }
        public int idParada { get; set; }
        public Coordenadas lugar { get; set; }
        public string nombre { get; set; }
        public string nombreCorto { get; set; }
        public List<KeyValuePair<string, LineaBusTiempo>> lineasYTiempo { get; set; }  

        public override string ToString()
        {
            string respuesta= "Parada "+nombre+" : ID "+id+" IDParada "+idParada+" Lugar: "+ lugar.longitud+" , "+lugar.latitud;
            return respuesta;
        }


    }
}
