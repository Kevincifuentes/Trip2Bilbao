using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Almacenamiento;
using Clases;

namespace WebAPI.Assemblers
{
    public class PuntoBiciAssembler
    {
        public PuntoBiciDTO assemble(puntos_bici h)
        {
            return new PuntoBiciDTO(h.nombre, h.latitud, h.longitud, h.estado, h.capacidad, h.codigoPostal);
        }

        public List<PuntoBiciDTO> assemble(List<puntos_bici> h)
        {
            List<PuntoBiciDTO> list = new List<PuntoBiciDTO>();
            foreach (puntos_bici var in h)
            {
                list.Add(assemble(var));
            }
            return list;
        }
    }
}