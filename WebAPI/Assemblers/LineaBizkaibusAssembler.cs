using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Almacenamiento;
using Clases;
using WebAPI.DTO;

namespace WebAPI.Assemblers
{
    public class LineaBizkaibusAssembler
    {
        public LineaBizkaibusDTO assemble(lineas_bizkaibus h)
        {
            return new LineaBizkaibusDTO(h.id, h.nombre, h.abreviatura);
        }

        public List<LineaBizkaibusDTO> assemble(List<lineas_bizkaibus> h)
        {
            List<LineaBizkaibusDTO> list = new List<LineaBizkaibusDTO>();
            foreach (lineas_bizkaibus var in h)
            {
                list.Add(assemble(var));
            }
            return list;
        }
    }
}