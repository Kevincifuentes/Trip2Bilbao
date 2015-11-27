using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clases;

namespace WrappersActiveMQ
{
    public class ParesLineasEuskotren
    {
        public ParesLineasEuskotren()
        {

        }
        public ParesLineasEuskotren(List<Clases.KeyValuePair<int, LineaEuskotren>> lista)
        {
            this.lista = lista;
        }
        public List<Clases.KeyValuePair<int, LineaEuskotren>> lista { get; set; }
    }
}
