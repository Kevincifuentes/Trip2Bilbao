using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clases;

namespace WrappersActiveMQ
{
    public class WrapperHospital
    {
        public WrapperHospital()
        {

        }
        public WrapperHospital(List<Hospital> lista)
        {
            this.lista = lista;
        }
        public List<Hospital> lista { get; set; }
    }
}
