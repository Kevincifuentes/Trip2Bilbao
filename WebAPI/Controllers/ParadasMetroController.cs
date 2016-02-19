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
    public class ParadasMetroController : ApiController
    {
        private ModeloContainer contexto;
        public ParadasMetroController()
        {
            contexto = new ModeloContainer();
        }

        [HttpGet]
        [ActionName("paradasmetro")]
        public IEnumerable<ParadaMetroDTO> ParadaMetro()
        {
            ParadaMetroAssembler fa = new ParadaMetroAssembler();
            return fa.assemble(contexto.paradas_metroSet.ToList());
        }

        [HttpGet]
        [ActionName("paradametro")]
        public IHttpActionResult GetParadaMetro(int id)
        {
            ParadaMetroAssembler fa = new ParadaMetroAssembler();
            paradas_metro temporal = contexto.paradas_metroSet.Where(h => h.id == id).FirstOrDefault<paradas_metro>();
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
            ParadaMetroAssembler fa = new ParadaMetroAssembler();
            List<paradas_metro> temporal = contexto.paradas_metroSet.Where(h => h.codigoPostal == id).ToList();
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
