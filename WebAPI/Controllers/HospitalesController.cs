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
    public class HospitalesController : ApiController
    {
        private ModeloContainer contexto;
        public HospitalesController()
        {
            contexto = new ModeloContainer();
        }

        [HttpGet]
        [ActionName("hospitales")]
        public IEnumerable<HospitalDTO> Hospitales()
        {
            HospitalAssembler fa = new HospitalAssembler();
            return fa.assemble(contexto.hospitalesSet.ToList());
        }

        [HttpGet]
        [ActionName("hospital")]
        public IHttpActionResult GetHospital(int id)
        {
            HospitalAssembler fa = new HospitalAssembler();
            hospitales temporal = contexto.hospitalesSet.Where(h => h.id == id).FirstOrDefault<hospitales>();
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
        public IHttpActionResult hospitalesCodigoPostal(int id)
        {
            HospitalAssembler fa = new HospitalAssembler();
            List<hospitales> temporal = contexto.hospitalesSet.Where(h => h.codigoPostal == id).ToList();
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
