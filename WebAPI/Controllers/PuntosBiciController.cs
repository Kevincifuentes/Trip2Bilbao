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
    public class PuntosBiciController : ApiController
    {
        private ModeloContainer contexto;
        public PuntosBiciController()
        {
            contexto = new ModeloContainer();
        }

        [HttpGet]
        [ActionName("puntosbici")]
        public IEnumerable<PuntoBiciDTO> PuntosBici()
        {
            PuntoBiciAssembler fa = new PuntoBiciAssembler();
            return fa.assemble(contexto.puntos_biciSet.ToList());
        }

        [HttpGet]
        [ActionName("puntobici")]
        public IHttpActionResult GetPuntoBici(int id)
        {
            PuntoBiciAssembler fa = new PuntoBiciAssembler();
            puntos_bici temporal = contexto.puntos_biciSet.Where(h => h.id == id).FirstOrDefault<puntos_bici>();
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
        [ActionName("codigo")]
        public IHttpActionResult puntosBiciCodigoPostal(int id)
        {
            PuntoBiciAssembler fa = new PuntoBiciAssembler();
            List<puntos_bici> temporal = contexto.puntos_biciSet.Where(h => h.codigoPostal == id).ToList();
            if (temporal.Count != 0)
            {
                return Ok(fa.assemble(temporal));
            }
            else
            {
                return NotFound();
            }

        }


    }
}
