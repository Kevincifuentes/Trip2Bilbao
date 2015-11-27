using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class ViajeEuskotren
    {
        public ViajeEuskotren()
        {
            
        }

        public ViajeEuskotren(string id, ParadaEuskotren p)
        {
            this.id = id;
            this.paradas = new List<KeyValuePair<int, ParadaEuskotren>>();
            this.paradas.Add(new KeyValuePair<int, ParadaEuskotren>(p.id, p));
            this.clavesParada = new List<int>();
            clavesParada.Add(p.id);
        }

        public string id { get; set; }
        public List<KeyValuePair<int, ParadaEuskotren>> paradas { get; set; }
        public List<int> clavesParada { get; set; } 
    }
}
