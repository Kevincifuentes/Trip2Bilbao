using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class LineaEuskotren
    {
        public LineaEuskotren()
        {
            
        }

        public LineaEuskotren(int id, string idAgencia, string abreviatura, string nombre, int tipo)
        {
            this.id = id;
            this.idAgencia = idAgencia;
            this.abreviatura = abreviatura;
            this.nombre = nombre;
            this.tipo = tipo;
            this.viajes = new List<KeyValuePair<string, ViajeEuskotren>>();
        }

        public int id { get; set; }
        public string idAgencia { get; set; }
        public string abreviatura { get; set; }
        public string nombre { get; set; }
        public int tipo { get; set; }
        public List<KeyValuePair<string, ViajeEuskotren>> viajes { get; set; } 

    }
}
