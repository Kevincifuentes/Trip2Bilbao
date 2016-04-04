using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.DTO
{
    public class LineaEuskotrenDTO
    {
        public LineaEuskotrenDTO(int id,  string nombre, string abreviatura)
        {
            this.id = id;
            this.nombre = nombre;
            this.abreviatura = abreviatura;
        }

        public int id { get; set; }
        public string abreviatura { get; set; }
        public string nombre { get; set; }
    }
}