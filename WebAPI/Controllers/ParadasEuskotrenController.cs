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

        [HttpGet]
        [ActionName("lineas")]
        public IHttpActionResult lineasEuskotren()
        {
            LineaEuskotrenAssembler fa = new LineaEuskotrenAssembler();
            List<lineas_euskotren> temporal = contexto.lineas_euskotrenSet.ToList();
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
        public IHttpActionResult paradasEuskotrenLinea(int id)
        {
            ParadaEuskotrenAssembler fa = new ParadaEuskotrenAssembler();
            List<paradas_euskotren> final = new List<paradas_euskotren>();
            lineas_euskotren primero = contexto.lineas_euskotrenSet.Where(l => l.id == id).FirstOrDefault<lineas_euskotren>();
            if (primero != null)
            {
                List<viajes_euskotren> temporal2 =
                primero.viajes_euskotren.ToList();
                List<viajes_euskotren> viajes = new List<viajes_euskotren>();
                foreach (viajes_euskotren t in temporal2)
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
                    foreach (viajes_parada_tiempos_euskotren t2 in viajes[0].viajes_parada_tiempos)
                    {
                        final.Add(t2.paradas_euskotren);
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
