using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clases;

namespace WrappersActiveMQ
{
    public class ParesLineasBilbobus
    {
        public ParesLineasBilbobus()
        {

        }
        public ParesLineasBilbobus(List<Clases.KeyValuePair<string, LineaBilbobus>> lista)
        {
            this.lista = lista;
        }
        public List<Clases.KeyValuePair<string, LineaBilbobus>> lista { get; set; }
    }
}
