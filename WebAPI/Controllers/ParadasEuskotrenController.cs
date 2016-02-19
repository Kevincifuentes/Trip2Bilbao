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
    public class ParadasEuskotrenController : ApiController
    {
        private ModeloContainer contexto;
        public ParadasEuskotrenController()
        {
            contexto = new ModeloContainer();
        }

        [HttpGet]
        [ActionName("paradaseuskotren")]
        public IEnumerable<ParadaEuskotrenDTO> ParadaBizkaibus()
        {
            ParadaEuskotrenAssembler fa = new ParadaEuskotrenAssembler();
            return fa.assemble(contexto.paradas_euskotrenSet.ToList());
        }

        [HttpGet]
        [ActionName("paradaeuskotren")]
        public IHttpActionResult GetParadaEuskotren(int id)
        {
            ParadaEuskotrenAssembler fa = new ParadaEuskotrenAssembler();
            paradas_euskotren temporal = contexto.paradas_euskotrenSet.Where(h => h.id == id).FirstOrDefault<paradas_euskotren>();
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
        public IHttpActionResult paradasEuskotrenCodigoPostal(int id)
        {
            ParadaEuskotrenAssembler fa = new ParadaEuskotrenAssembler();
            List<paradas_euskotren> temporal = contexto.paradas_euskotrenSet.Where(h => h.codigoPostal == id).ToList();
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
