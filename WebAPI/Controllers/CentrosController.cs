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
    public class CentrosController : ApiController
    {
        private ModeloContainer contexto;
        public CentrosController()
        {
            contexto = new ModeloContainer();
        }

        [HttpGet]
        [ActionName("centros")]
        public IEnumerable<CentroSaludDTO> Centros()
        {
            CentroAssembler fa = new CentroAssembler();
            return fa.assemble(contexto.centros_de_saludSet.ToList());
        }

        [HttpGet]
        [ActionName("centro")]
        public IHttpActionResult GetCentros(int id)
        {
            CentroAssembler fa = new CentroAssembler();
            centros_de_salud temporal = contexto.centros_de_saludSet.Where(h => h.id == id).FirstOrDefault<centros_de_salud>();
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
        public IHttpActionResult centrosCodigoPostal(int id)
        {
            CentroAssembler fa = new CentroAssembler();
            List<centros_de_salud> temporal = contexto.centros_de_saludSet.Where(h => h.codigoPostal == id).ToList();
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
