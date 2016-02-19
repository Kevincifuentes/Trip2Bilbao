using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Almacenamiento;
using Clases;

namespace WebAPI.Assemblers
{
    public class ParadaMetroAssembler
    {
        public ParadaMetroDTO assemble(paradas_metro h)
        {
            return new ParadaMetroDTO(h.nombre, h.latitud, h.longitud, h.codigoPostal, h.codigoParada);
        }

        public List<ParadaMetroDTO> assemble(List<paradas_metro> h)
        {
            List<ParadaMetroDTO> list = new List<ParadaMetroDTO>();
            foreach (paradas_metro var in h)
            {
                list.Add(assemble(var));
            }
            return list;
        }
    }
}