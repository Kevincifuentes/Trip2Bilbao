using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clases;

namespace WrappersActiveMQ
{
    public class WrapperEvento
    {
        public WrapperEvento()
        {

        }
        public WrapperEvento(List<Evento> lista)
        {
            this.lista = lista;
        }
        public List<Evento> lista { get; set; }
    }
}
