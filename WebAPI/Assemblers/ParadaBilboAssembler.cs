using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Almacenamiento;
using Clases;

namespace WebAPI.Assemblers
{
    public class ParadaBilboAssembler
    {
        public ParadaBilboDTO assemble(paradas_bilbobus h)
        {
            return new ParadaBilboDTO(h.nombre, h.latitud, h.longitud, h.abreviatura, h.codigoPostal);
        }

        public List<ParadaBilboDTO> assemble(List<paradas_bilbobus> h)
        {
            List<ParadaBilboDTO> list = new List<ParadaBilboDTO>();
            foreach (paradas_bilbobus var in h)
            {
                list.Add(assemble(var));
            }
            return list;
        }
    }
}