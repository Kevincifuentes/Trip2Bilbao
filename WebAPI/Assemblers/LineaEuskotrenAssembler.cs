using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Almacenamiento;
using Clases;
using WebAPI.DTO;

namespace WebAPI.Assemblers
{
    public class LineaEuskotrenAssembler
    {
        public LineaEuskotrenDTO assemble(lineas_euskotren h)
        {
            return new LineaEuskotrenDTO(h.id, h.nombre, h.abreviatura);
        }

        public List<LineaEuskotrenDTO> assemble(List<lineas_euskotren> h)
        {
            List<LineaEuskotrenDTO> list = new List<LineaEuskotrenDTO>();
            foreach (lineas_euskotren var in h)
            {
                list.Add(assemble(var));
            }
            return list;
        }
    }
}