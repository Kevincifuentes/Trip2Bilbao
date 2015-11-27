using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class LineaBilbobus
    {

        public LineaBilbobus()
        {
            
        }

        public LineaBilbobus(string id, string idAgencia, string abreviatura, string nombre, int tipo)
        {
            this.id = id;
            this.idAgencia = idAgencia;
            this.abreviatura = abreviatura;
            this.nombre = nombre;
            this.tipo = tipo;
            viajes= new List<KeyValuePair<int, ViajeBilbobus>>();
        }

        public string id { get; set; }
        public string idAgencia { get; set; }
        public string abreviatura { get; set; }
        public string nombre { get; set; }
        public int tipo { get; set; }
        //Distintas horas pero mismo viaje
        public List<KeyValuePair<int, ViajeBilbobus>> viajes { get; set; } 
    }
}
