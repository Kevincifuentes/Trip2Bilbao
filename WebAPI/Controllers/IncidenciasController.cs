using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Almacenamiento;
using Clases;
using WebAPI.Assemblers;

namespace WebAPI.Controllers
{
    public class IncidenciasController : ApiController
    {
        private ModeloContainer contexto;
        public IncidenciasController()
        {
            contexto = new ModeloContainer();
        }

        [HttpGet]
        [ActionName("incidencias")]
        public IEnumerable<IncidenciaDTO> Incidencias()
        {
            IncidenciaAssembler fa = new IncidenciaAssembler();
            return fa.assemble(contexto.incidenciasSet.ToList());
        }

        [HttpGet]
        [ActionName("incidencia")]
        public IHttpActionResult GetIncidencias(int id)
        {
            IncidenciaAssembler fa = new IncidenciaAssembler();
            incidencias temporal = contexto.incidenciasSet.Where(h => h.id == id).FirstOrDefault<incidencias>();
            if (temporal != null)
            {
                return Ok(fa.assemble(temporal));
            }
            else
            {
                return NotFound();
            }
            
        }

        [HttpGet]
        [ActionName("fecha")]
        public IHttpActionResult incidenciasFecha()
        {
            IncidenciaAssembler fa = new IncidenciaAssembler();
            List<incidencias> temporal = contexto.incidenciasSet.ToList();
            List<incidencias> valida = new List<incidencias>();
            int id = -1;
            foreach (incidencias variable in temporal)
            {
                if (variable.id != id && variable.fechaInicio.Ticks < DateTime.Now.Ticks && variable.fechaFin.Ticks > DateTime.Now.Ticks)
                {
                    variable.latitud = Math.Round(variable.latitud, 14);
                    variable.longitud = Math.Round(variable.longitud, 14);
                    valida.Add(variable);
                    id = variable.id;
                }
            }

            if (temporal.Count != 0)
            {
                return Ok(fa.assemble(valida));
            }
            else
            {
                return NotFound();
            }

        }

        [HttpGet]
        [ActionName("codigo")]
        public IHttpActionResult incidenciasCodigo(int id)
        {
            IncidenciaAssembler fa = new IncidenciaAssembler();
            List<incidencias> temporal = contexto.incidenciasSet.Where(i => i.codigoPostal == id).ToList();
            List<incidencias> valida = new List<incidencias>();
            int identificador = -1;
            foreach (incidencias variable in temporal)
            {
                if (variable.id != identificador && variable.fechaInicio.Ticks < DateTime.Now.Ticks && variable.fechaFin.Ticks > DateTime.Now.Ticks)
                {
                    variable.latitud = Math.Round(variable.latitud, 14);
                    variable.longitud = Math.Round(variable.longitud, 14);
                    valida.Add(variable);
                    identificador = variable.id;
                }
            }

            if (temporal.Count != 0)
            {
                return Ok(fa.assemble(valida));
            }
            else
            {
                return NotFound();
            }

        }


    }
}
