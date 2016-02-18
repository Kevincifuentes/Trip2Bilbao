using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Almacenamiento;
using Clases;

namespace WebAPI.Assemblers
{
    public class ParkingAssembler
    {
        public ParkingDTO assemble(parkings h)
        {
            return new ParkingDTO(h.nombre, h.tipo, h.latitud, h.longitud, h.estado, h.capacidad, h.codigoPostal);
        }

        public List<ParkingDTO> assemble(List<parkings> h)
        {
            List<ParkingDTO> list = new List<ParkingDTO>();
            foreach (parkings var in h)
            {
                list.Add(assemble(var));
            }
            return list;
        }
    }
}