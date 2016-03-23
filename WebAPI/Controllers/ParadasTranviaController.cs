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
    public class ParadasTranviaController : ApiController
    {
        private ModeloContainer contexto;
        public ParadasTranviaController()
        {
            contexto = new ModeloContainer();
        }

        [HttpGet]
        [ActionName("paradastranvia")]
        public IEnumerable<ParadaTranviaDTO> ParadaTranvia()
        {
            ParadaTranviaAssembler fa = new ParadaTranviaAssembler();
            return fa.assemble(contexto.paradas_tranviaSet.ToList());
        }

        [HttpGet]
        [ActionName("paradatranvia")]
        public IHttpActionResult GetParadaTranvia(int id)
        {
            ParadaTranviaAssembler fa = new ParadaTranviaAssembler();
            paradas_tranvia temporal = contexto.paradas_tranviaSet.Where(h => h.Id == id).FirstOrDefault<paradas_tranvia>();
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
        public IHttpActionResult paradasMetroCodigoPostal(int id)
        {
            ParadaTranviaAssembler fa = new ParadaTranviaAssembler();
            List<paradas_tranvia> temporal = contexto.paradas_tranviaSet.Where(h => h.codigoPostal == id).ToList();
            if (temporal.Count != 0)
            {
                return Ok(fa.assemble(temporal));
            }
            else
            {
                return NotFound();
            }

        }

        [HttpGet]
        [ActionName("linea")]
        public IHttpActionResult paradasLinea()
        {
            ParadaTranviaAssembler fa = new ParadaTranviaAssembler();
            List<paradas_tranvia> temporal = contexto.paradas_tranviaSet.OrderBy(p => p.orden).ToList();
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
