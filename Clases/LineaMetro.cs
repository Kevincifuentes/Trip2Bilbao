using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class LineaMetro
    {
        public LineaMetro()
        {
            
        }

        public LineaMetro(string id, string abreviatura, string nombre, int tipo, string colorRuta)
        {
            this.id = id;
            this.abreviatura = abreviatura;
            this.nombre = nombre;
            this.tipo = tipo;
            this.colorRuta = colorRuta;
            this.viajes = new List<KeyValuePair<int, ViajeMetro>>();
        }

        public string id { get; set; }
        public string abreviatura { get; set; }
        public string nombre { get; set; }
        public int tipo { get; set; }
        public string colorRuta { get; set; }
        public List<KeyValuePair<int, ViajeMetro>> viajes { get; set; }
    }
}
