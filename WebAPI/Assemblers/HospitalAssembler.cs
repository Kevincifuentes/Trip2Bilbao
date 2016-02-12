using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Almacenamiento;
using Clases;

namespace WebAPI.Assemblers
{
    public class HospitalAssembler
    {
        public HospitalDTO assemble(hospitales h)
        {
            return new HospitalDTO(h.nombre, h.direccion, h.latitud, h.longitud, h.region, h.ciudad, h.web, h.telefono);
        }

        public List<HospitalDTO> assemble(List<hospitales> h)
        {
            List<HospitalDTO> list = new List<HospitalDTO>();
            foreach (hospitales var in h)
            {
                list.Add(assemble(var));
            }
            return list;
        }
    }
}