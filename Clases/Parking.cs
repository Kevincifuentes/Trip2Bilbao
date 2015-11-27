using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class Parking
    {
        public Parking()
        {

        }
      
        public int id { get; set; }
        public string nombre { get; set; }
        public DateTime fecha { get; set; }
        public string estado { get; set; }
        public double ocupacion { get; set;}
        public double porcentaje { get; set; }
        public Coordenadas latlong { get; set; }
        public List<Entrada> entradas { get; set; }
        public int capacidad { get; set; }
        public string tipo {  get; set; }
        public List<Tarifa> tarifas { get; set; }

        public override string ToString()
        {
            string respuesta = "Parking " + id +" "+ nombre+" : " + "Última modificación: "+ fecha+ " Estado: "+ estado+" Ocupación: "+ocupacion+" Porcentaje: "+ porcentaje+"\n";
            if(latlong!=null)
            { respuesta = respuesta + "Coordenadas: " + latlong.longitud + " , " + latlong.latitud + " Capacidad: " + capacidad + " Tipo: " + tipo + "\n"; }
            
            respuesta = respuesta + "----Entradas----";
            if(entradas!=null)
            {
                foreach (Entrada e in entradas)
                {
                    respuesta = respuesta + e.nombre + " Punto de entrada: " + e.puntoEntrada.longitud + " , " + e.puntoEntrada.latitud + "\n";
                }
            }
           
            if(tarifas!=null)
            {
                foreach (Tarifa t in tarifas)
                {
                    respuesta = respuesta + t.tipo + " Descripción: " + t.descripcion + " Actualización: " + t.actualizacion + " Zona: " + t.zona + "\n";
                }
            }
            return respuesta;
        }
    }
}
