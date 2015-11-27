using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clases;

namespace WrappersActiveMQ
{
    public class WrapperCentroSalud
    {
        public WrapperCentroSalud()
        {

        }
        public WrapperCentroSalud(List<CentroSalud> lista)
        {
            this.lista = lista;
        }
        public List<CentroSalud> lista { get; set; }
    }
}
