using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clases;

namespace WrappersActiveMQ
{
    public class ParesParking
    {
        public ParesParking()
        {
            
        }
        public ParesParking(List<Clases.KeyValuePair<int, Parking>> lista)
        {
            this.lista = lista;
        }
        public List<Clases.KeyValuePair<int, Parking>> lista { get; set; }
    }
}
