using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class ViajeBizkaibus
    {
        public ViajeBizkaibus()
        {
            
        }

        public ViajeBizkaibus(int id, ParadaBizkaibus p)
        {
            this.id = id;
            this.paradas= new List<KeyValuePair<int, ParadaBizkaibus>>();
            paradas.Add(new KeyValuePair<int, ParadaBizkaibus>(p.id, p));
            this.clavesParada = new List<int>();
            clavesParada.Add(p.id);
        }

        public int id { get; set; }
        public List<KeyValuePair<int, ParadaBizkaibus>> paradas { get; set; }
        public List<int> clavesParada { get; set; } 
    }
}
