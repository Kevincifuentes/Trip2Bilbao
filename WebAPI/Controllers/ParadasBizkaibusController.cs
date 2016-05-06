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
    public class ParadasBizkaibusController : ApiController
    {
        private ModeloContainer contexto;
        public ParadasBizkaibusController()
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

        [HttpGet]
        [ActionName("lineas")]
        public IHttpActionResult lineasBizkaibus()
        {
            LineaBizkaibusAssembler fa = new LineaBizkaibusAssembler();
            List<lineas_bizkaibus> temporal = contexto.lineas_bizkaibusSet.OrderBy(b => b.abreviatura).ToList();
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
        public IHttpActionResult paradaBizkaibusLinea(int id)
        {
            ParadaBizkaibusAssembler fa = new ParadaBizkaibusAssembler();
            List<paradas_bizkaibus> final = new List<paradas_bizkaibus>();
            lineas_bizkaibus primero = contexto.lineas_bizkaibusSet.Where(l => l.id == id).FirstOrDefault<lineas_bizkaibus>();
            if (primero != null)
            {
                List<viajes_bizkaibus> temporal2 =
                primero.viajes_bizkaibus.ToList();
                List<viajes_bizkaibus> viajes = new List<viajes_bizkaibus>();
                List<viajes_bizkaibus> viajesDetodas = new List<viajes_bizkaibus>();
                foreach (viajes_bizkaibus t in temporal2)
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
                    else
                    {
                        viajesDetodas.Add(t);
                    }
                }
                if (viajes.Count == 0)
                {
                    if (temporal2.Count > 0)
                    {
                        foreach (viajes_parada_tiempos_bizkaibus t2 in viajesDetodas[0].viajes_parada_tiempos_bizkaibus)
                        {
                            final.Add(t2.paradas_bizkaibus);
                        }
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
                else
                {
                    foreach (viajes_parada_tiempos_bizkaibus t2 in viajes[0].viajes_parada_tiempos_bizkaibus)
                    {
                        final.Add(t2.paradas_bizkaibus);
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
