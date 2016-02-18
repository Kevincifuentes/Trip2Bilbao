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
    public class ParadasBilboController : ApiController
    {
        private ModeloContainer contexto;
        public ParadasBilboController()
        {
            contexto = new ModeloContainer();
        }

        [HttpGet]
        [ActionName("paradasbilbo")]
        public IEnumerable<ParadaBilboDTO> ParadasBilbo()
        {
            ParadaBilboAssembler fa = new ParadaBilboAssembler();
            return fa.assemble(contexto.paradas_bilbobusSet.ToList());
        }

        [HttpGet]
        [ActionName("paradabilbo")]
        public IHttpActionResult GetParadaBilbo(int id)
        {
            ParadaBilboAssembler fa = new ParadaBilboAssembler();
            paradas_bilbobus temporal = contexto.paradas_bilbobusSet.Where(h => h.id == id).FirstOrDefault<paradas_bilbobus>();
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
        public IHttpActionResult paradasBilboCodigoPostal(int id)
        {
            ParadaBilboAssembler fa = new ParadaBilboAssembler();
            List<paradas_bilbobus> temporal = contexto.paradas_bilbobusSet.Where(h => h.codigoPostal == id).ToList();
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
