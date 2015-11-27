using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clases;

namespace WrappersActiveMQ
{
    public class ParesBilbobus
    {
        public ParesBilbobus()
        {

        }
        public ParesBilbobus(List<Clases.KeyValuePair<int, ParadaBilbo>> lista)
        {
            this.lista = lista;
        }
        public List<Clases.KeyValuePair<int, ParadaBilbo>> lista { get; set; }
    }
}
