using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Almacenamiento;
using Clases;

namespace WebAPI.Assemblers
{
    public class ParadaBizkaibusAssembler
    {
        public ParadaBizkaibusDTO assemble(paradas_bizkaibus h)
        {
            return new ParadaBizkaibusDTO(h.nombre, h.latitud, h.longitud, h.codigoPostal);
        }

        public List<ParadaBizkaibusDTO> assemble(List<paradas_bizkaibus> h)
        {
            List<ParadaBizkaibusDTO> list = new List<ParadaBizkaibusDTO>();
            foreach (paradas_bizkaibus var in h)
            {
                list.Add(assemble(var));
            }
            return list;
        }
    }
}