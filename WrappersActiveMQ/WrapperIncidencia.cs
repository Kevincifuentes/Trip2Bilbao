using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clases;

namespace WrappersActiveMQ
{
    public class WrapperIncidencia
    {
        public WrapperIncidencia()
        {

        }

        public WrapperIncidencia(List<Incidencia> lista)
        {
            this.lista = lista;
        }

        public List<Incidencia> lista { get; set; }
    }
}
