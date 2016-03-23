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

        [HttpGet]
        [ActionName("linea")]
        public IHttpActionResult paradasBilboLinea(string id)
        {
            ParadaBilboAssembler fa = new ParadaBilboAssembler();
            List<paradas_bilbobus> final = new List<paradas_bilbobus>();
            if (id.Length == 1)
            {
                id = "0" + id;
            }
            lineas_bilbobus primero = contexto.lineas_bilbobusSet.Where(l => l.idLinea == id).FirstOrDefault<lineas_bilbobus>();
            if (primero != null)
            {
                List<viajes_bilbobus> temporal2 =
                primero.viajes_bilbobus.ToList();
                List<viajes_bilbobus> viajes = new List<viajes_bilbobus>();
                foreach (viajes_bilbobus t in temporal2)
                {
                    string[] tiempo1 = t.tiempoInicio.Split(':');
                    int horaI = int.Parse(tiempo1[0]);
                    int minutosI = int.Parse(tiempo1[1]);
                    string[] tiempo2 = t.tiempoFin.Split(':');
                    int horaF = int.Parse(tiempo2[0]);
                    int minutosF = int.Parse(tiempo2[1]);
                    if (DateTime.Now.TimeOfDay < new TimeSpan(horaI, minutosI, 0))
                    {
                        viajes.Add(t);
                    }
                }
                if (viajes.Count == 0)
                {
                    return NotFound();
                }
                else
                {
                    foreach (viajes_parada_tiempos_bilbobus t2 in viajes[0].viajes_parada_tiempos_bilbobus)
                    {
                        final.Add(t2.paradas_bilbobus);
                    }

                    if (final.Count != 0)
                    {
                        return Ok(fa.assemble(final));
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
            else
            {
                return NotFound(); 
            }
        }

    }
}
