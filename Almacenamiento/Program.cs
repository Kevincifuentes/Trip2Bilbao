using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace Almacenamiento
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new ModeloContainer();

            var hostpital = new hospitales();

            var lineasmetro2 = context.lineas_metroSet.ToList();

            foreach (var lineasMetro in lineasmetro2)
            {
                Console.WriteLine(lineasMetro.viajes_metro.First().id);
            }

            var lineasmetro = context.lineas_metroSet.Include(lm => lm.viajes_metro).ToList();


            context.hospitalesSet.Add(hostpital);

            context.SaveChanges();


            var hospitales = context.hospitalesSet.Where(h => h.nombre == "basurto" && h.calle == "autonomia");


        }
    }
}
