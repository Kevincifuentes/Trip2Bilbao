using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Almacenamiento;
using Clases;

namespace WebAPI.Assemblers
{
    public class FarmaciaAssembler
    {

        public FarmaciaDTO assemble(farmacias f)
        {
            return new FarmaciaDTO(f.nombre, f.codigoPostal, f.provincia, f.direccionAbreviada, f.ciudad, f.url, f.latitud, f.longitud, f.telefono);
        }

        public List<FarmaciaDTO> assemble(List<farmacias> f)
        {
            List<FarmaciaDTO> list = new List<FarmaciaDTO>();
            foreach (farmacias var in f)
            {
                list.Add(assemble(var));
            }
            return list;
        }
    }
}