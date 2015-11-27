using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clases;

namespace WrappersActiveMQ
{
    public class ParesBizkaibus
    {
        public ParesBizkaibus()
        {

        }
        public ParesBizkaibus(List<Clases.KeyValuePair<int, ParadaBizkaibus>> lista)
        {
            this.lista = lista;
        }
        public List<Clases.KeyValuePair<int, ParadaBizkaibus>> lista { get; set; }
    }
}
