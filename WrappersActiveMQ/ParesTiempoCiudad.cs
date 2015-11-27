using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clases;

namespace WrappersActiveMQ
{
    public class ParesTiempoCiudad
    {
        public ParesTiempoCiudad()
        {
            
        }
        public ParesTiempoCiudad(List<Clases.KeyValuePair<string, TiempoCiudad>> lista)
        {
            this.lista = lista;
        }
        public List<Clases.KeyValuePair<string, TiempoCiudad>> lista { get; set; }
    }
}
