using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class ViajeMetro
    {
        public ViajeMetro()
        {
            
        }

        public ViajeMetro(int id, ParadaMetro pm, int secuencia)
        {
            this.id = id;
            this.paradasList = new List<KeyValuePair<int, ParadaMetro>>();
            paradasList.Add(new KeyValuePair<int, ParadaMetro>(secuencia, pm));
        }

        public int id { get; set; }
        public List<KeyValuePair<int, ParadaMetro>> paradasList { get; set; }

    }
}
