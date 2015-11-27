using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clases;

namespace WrappersActiveMQ
{
    public class WrapperBicicleta
    {
        public WrapperBicicleta()
        {

        }
        public WrapperBicicleta(List<PuntoBici> lista)
        {
            this.lista = lista;
        }
        public List<PuntoBici> lista { get; set; }
    }
}
