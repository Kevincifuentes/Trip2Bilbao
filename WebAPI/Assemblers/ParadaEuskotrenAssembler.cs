using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Almacenamiento;
using Clases;

namespace WebAPI.Assemblers
{
    public class ParadaEuskotrenAssembler
    {
        public ParadaEuskotrenDTO assemble(paradas_euskotren h)
        {
            return new ParadaEuskotrenDTO(h.nombre, h.latitud, h.longitud, h.codigoPostal, h.codigoParada);
        }

        public List<ParadaEuskotrenDTO> assemble(List<paradas_euskotren> h)
        {
            List<ParadaEuskotrenDTO> list = new List<ParadaEuskotrenDTO>();
            foreach (paradas_euskotren var in h)
            {
                list.Add(assemble(var));
            }
            return list;
        }
    }
}