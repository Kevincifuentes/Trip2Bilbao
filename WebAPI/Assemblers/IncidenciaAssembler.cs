using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Almacenamiento;
using Clases;

namespace WebAPI.Assemblers
{
    public class IncidenciaAssembler
    {
        public IncidenciaDTO assemble(incidencias cds)
        {
            return new IncidenciaDTO(cds.tipo, cds.descripcion, cds.latitud, cds.longitud, cds.fechaInicio, cds.fechaFin);
        }

        public List<IncidenciaDTO> assemble(List<incidencias> cdss)
        {
            List<IncidenciaDTO> list = new List<IncidenciaDTO>();
            foreach (incidencias var in cdss)
            {
                list.Add(assemble(var));
            }
            return list;
        }
    }
}