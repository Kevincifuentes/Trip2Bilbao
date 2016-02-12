using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Almacenamiento;
using Clases;

namespace WebAPI.Assemblers
{
    public class CentroAssembler
    {
        public CentroSaludDTO assemble(centros_de_salud cds)
        {
            return new CentroSaludDTO(cds.nombre, cds.direcionCompleta, cds.codigoPostal, cds.provincia, cds.region, cds.horario, cds.ciudad, cds.urlAdicional, cds.latitud, cds.longitud, cds.web, cds.telefono);
        }

        public List<CentroSaludDTO> assemble(List<centros_de_salud> cdss)
        {
            List<CentroSaludDTO> list = new List<CentroSaludDTO>();
            foreach (centros_de_salud var in cdss)
            {
                list.Add(assemble(var));
            }
            return list;
        }
    }
}