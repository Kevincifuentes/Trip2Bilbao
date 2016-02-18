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
        [ActionName("paradasbizkaibus")]
        public IEnumerable<ParadaBizkaibusDTO> ParadaBizkaibus()
        {
            ParadaBizkaibusAssembler fa = new ParadaBizkaibusAssembler();
            return fa.assemble(contexto.paradas_bizkaibusSet.ToList());
        }

        [HttpGet]
        [ActionName("paradabizkaibus")]
        public IHttpActionResult GetParadaBizkaibus(int id)
        {
            ParadaBizkaibusAssembler fa = new ParadaBizkaibusAssembler();
            paradas_bizkaibus temporal = contexto.paradas_bizkaibusSet.Where(h => h.id == id).FirstOrDefault<paradas_bizkaibus>();
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
        public IHttpActionResult paradasBizkaibusCodigoPostal(int id)
        {
            ParadaBizkaibusAssembler fa = new ParadaBizkaibusAssembler();
            List<paradas_bizkaibus> temporal = contexto.paradas_bizkaibusSet.Where(h => h.codigoPostal == id).ToList();
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
