using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clases;

namespace WrappersActiveMQ
{
    public class ParesLineasBizkaibus
    {
        public ParesLineasBizkaibus()
        {

        }
        public ParesLineasBizkaibus(List<Clases.KeyValuePair<int, LineaBizkaibus>> lista)
        {
            this.lista = lista;
        }
        public List<Clases.KeyValuePair<int, LineaBizkaibus>> lista { get; set; }
    }
}
