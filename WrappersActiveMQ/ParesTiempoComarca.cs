using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clases;

namespace WrappersActiveMQ
{
    public class ParesTiempoComarca
    {
        public ParesTiempoComarca()
        {

        }
        public ParesTiempoComarca(List<Clases.KeyValuePair<string, TiempoComarca>> lista)
        {
            this.lista = lista;
        }
        public List<Clases.KeyValuePair<string, TiempoComarca>> lista { get; set; }
    }
}
