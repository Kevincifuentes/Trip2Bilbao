using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clases;

namespace WrappersActiveMQ
{
    public class ParesLineaMetro
    {
        public ParesLineaMetro()
        {

        }
        public ParesLineaMetro(List<Clases.KeyValuePair<string, LineaMetro>> lista)
        {
            this.lista = lista;
        }
        public List<Clases.KeyValuePair<string, LineaMetro>> lista { get; set; }
    }
}
