using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class ViajeBilbobus
    {

        public ViajeBilbobus()
        {
            
        }

        public ViajeBilbobus(int id, ParadaBilbo p)
        {
            this.id = id;
            this.paradas= new List<KeyValuePair<int, ParadaBilbo>>();
            this.paradas.Add(new KeyValuePair<int, ParadaBilbo>(p.id, p));
            this.clavesParada = new List<int>();
            clavesParada.Add(p.id);
        }

        public int id { get; set; }
        public List<KeyValuePair<int, ParadaBilbo>> paradas { get; set; }
        public List<int> clavesParada { get; set; } 
    }
}
