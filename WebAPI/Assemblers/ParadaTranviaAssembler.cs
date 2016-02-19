using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Almacenamiento;
using Clases;

namespace WebAPI.Assemblers
{
    public class ParadaTranviaAssembler
    {
        public ParadaTranviaDTO assemble(paradas_tranvia h)
        {
            return new ParadaTranviaDTO(h.nombre, h.latitud, h.longitud, h.codigoPostal, h.descripcion);
        }

        public List<ParadaTranviaDTO> assemble(List<paradas_tranvia> h)
        {
            List<ParadaTranviaDTO> list = new List<ParadaTranviaDTO>();
            foreach (paradas_tranvia var in h)
            {
                list.Add(assemble(var));
            }
            return list;
        }
    }
}