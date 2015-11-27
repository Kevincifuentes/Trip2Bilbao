using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clases;

namespace WrappersActiveMQ
{
    public class WrapperMantenimiento
    {
        public WrapperMantenimiento()
        {

        }
        public WrapperMantenimiento(List<Mantenimiento> lista)
        {
            this.lista = lista;
        }
        public List<Mantenimiento> lista { get; set; }
    }
}
