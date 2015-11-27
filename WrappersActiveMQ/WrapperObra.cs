using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clases;

namespace WrappersActiveMQ
{
    public class WrapperObra
    {
        public WrapperObra()
        {

        }
        public WrapperObra(List<Obra> lista)
        {
            this.lista = lista;
        }
        public List<Obra> lista { get; set; }
    }
}
