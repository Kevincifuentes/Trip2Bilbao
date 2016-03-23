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
    public class ParkingsController : ApiController
    {
        private ModeloContainer contexto;
        public ParkingsController()
        {
            contexto = new ModeloContainer();
        }

        [HttpGet]
        [ActionName("parkings")]
        public IEnumerable<ParkingDTO> Parkings()
        {
            ParkingAssembler fa = new ParkingAssembler();
            return fa.assemble(contexto.parkingsSet.ToList());
        }

        [HttpGet]
        [ActionName("parking")]
        public IHttpActionResult GetParking(int id)
        {
            ParkingAssembler fa = new ParkingAssembler();
            parkings temporal = contexto.parkingsSet.Where(h => h.id == id).FirstOrDefault<parkings>();
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
        public IHttpActionResult parkingsCodigoPostal(int id)
        {
            ParkingAssembler fa = new ParkingAssembler();
            List<parkings> temporal = contexto.parkingsSet.Where(h => h.codigoPostal == id).ToList();
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
