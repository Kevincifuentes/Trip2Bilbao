using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WrappersActiveMQ
{
    public class WrapperParkingDeusto
    {
        public WrapperParkingDeusto()
        {

        }
        public WrapperParkingDeusto(int dbs, int general)
        {
            this.contadorDBS = dbs;
            this.contadorGeneral = general;
        }
        public int contadorDBS { get; set; }
        public int contadorGeneral { get; set; }
    }
}
