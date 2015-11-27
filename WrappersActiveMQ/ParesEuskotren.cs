using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clases;

namespace WrappersActiveMQ
{
    public class ParesEuskotren
    {
        public ParesEuskotren()
        {

        }
        public ParesEuskotren(List<Clases.KeyValuePair<int, ParadaEuskotren>> lista)
        {
            this.lista = lista;
        }
        public List<Clases.KeyValuePair<int, ParadaEuskotren>> lista { get; set; }
    }
}
