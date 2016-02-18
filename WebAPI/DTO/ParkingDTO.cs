using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class ParkingDTO
    {
        public ParkingDTO()
        {
            
        }

        public ParkingDTO(string nombreParking, string tipo, double latitud, double longitud, string estado, int capacidad, int codigoPostal)
        {
            this.nombreParking = nombreParking;
            this.tipo = tipo;
            this.estado = estado;
            this.capacidad = capacidad;
            this.latitud = latitud;
            this.longitud = longitud;
            this.codigoPostal = codigoPostal;
        }

        public string nombreParking { get; set; }
        public string tipo { get; set; }
        public string estado { get; set; }
        public int capacidad { get; set; }
        public double latitud { get; set; }
        public double longitud { get; set; }
        public double codigoPostal { get; set; }

    }
}
