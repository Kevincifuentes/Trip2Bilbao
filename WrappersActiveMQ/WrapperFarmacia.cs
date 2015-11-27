using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clases;

namespace WrappersActiveMQ
{
    public class WrapperFarmacia
    {
        public WrapperFarmacia()
        {

        }
        public WrapperFarmacia(List<Farmacia> lista)
        {
            this.lista = lista;
        }
        public List<Farmacia> lista { get; set; }
    }
}
