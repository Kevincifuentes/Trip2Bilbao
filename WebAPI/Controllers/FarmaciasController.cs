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
    public class FarmaciasController : ApiController
    {
        private ModeloContainer contexto;
        public FarmaciasController()
        {
            contexto = new ModeloContainer();
        }

        [HttpGet]
        [ActionName("farmacias")]
        public IEnumerable<FarmaciaDTO> Farmacias()
        {
            FarmaciaAssembler fa = new FarmaciaAssembler();
            return fa.assemble(contexto.farmaciasSet.ToList());
        }

        [HttpGet]
        [ActionName("farmacia")]
        public IHttpActionResult GetFarmacia(int id)
        {
            FarmaciaAssembler fa = new FarmaciaAssembler();
            farmacias temporal = contexto.farmaciasSet.Where(h => h.id == id).FirstOrDefault<farmacias>();
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
        public IHttpActionResult farmaciasCodigoPostal(int id)
        {
            FarmaciaAssembler fa = new FarmaciaAssembler();
            List<farmacias> temporal = contexto.farmaciasSet.Where(h => h.codigoPostal == id).ToList();
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
