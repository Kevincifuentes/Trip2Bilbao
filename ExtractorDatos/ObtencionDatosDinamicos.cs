using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Clases;

namespace ExtractorDatos
{
    public class ObtencionDatosDinamicos
    {
        private InformacionEstatica estatico;
        private InformacionDinamica dinamico;

        public ObtencionDatosDinamicos()
        {
            
            dinamico = new InformacionDinamica();
            estatico = new InformacionEstatica();
            Thread incidencias = new Thread(new ThreadStart(this.obtenerIncidencias));
            Thread obtenerBicis = new Thread(new ThreadStart(this.obtenerBicis));
            Thread obtenerParkings = new Thread(new ThreadStart(this.obtenerParkings));
            Thread deusto = new Thread(new ThreadStart(this.obtenerDeusto));
            Thread obtenerBilbo = new Thread(new ThreadStart(this.obtenerBilbobus));
            Thread tiempo = new Thread(new ThreadStart(this.obtenerTiempoBilbao));

            obtenerBilbo.Start();
            incidencias.Start();
            obtenerBicis.Start();
            obtenerParkings.Start();
            deusto.Start();
            tiempo.Start();
        }

        public void obtenerIncidencias()
        {
            while (true)
            {
                Thread.Sleep(5000);
                dinamico.eventosTrafico();
                dinamico.incidenciasVariasTrafico();
                dinamico.mantenimientoTrafico();
                dinamico.obrasTrafico();
                dinamico.descargaIncidencias = DateTime.Now;
                InformacionEstatica.emisor.enviarIncidencias<List<Evento>, Evento>(dinamico.eventos, dinamico.descargaIncidencias);
                InformacionEstatica.emisor.enviarIncidencias<List<Obra>, Obra>(dinamico.obras, dinamico.descargaIncidencias);
                InformacionEstatica.emisor.enviarIncidencias<List<Incidencia>, Incidencia>(dinamico.incidencias, dinamico.descargaIncidencias);
                InformacionEstatica.emisor.enviarIncidencias<List<Mantenimiento>, Mantenimiento>(dinamico.mantenimientos, dinamico.descargaIncidencias);

                //Espera un Dia
                Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>Fin Incidencias");
                Thread.Sleep(86400000);
            }
            
        }

        public void obtenerBicis()
        {
            while (true)
            {
                Thread.Sleep(5000);
                dinamico.bicicletas();
                InformacionEstatica.emisor.enviarBicicletas(dinamico.puntosBicisList, dinamico.descargaBicis);

                //Espera 45 segundos
                Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>Fin Bicis");
                Thread.Sleep(45000);
            }
        }

        public void obtenerParkings()
        {
            while (true)
            {
               
               Thread.Sleep(5000);
               estatico.obtenerInformacionParkings();
               InformacionEstatica.emisor.enviarParkings(estatico.parkings, estatico.descargaParkings);
                
               //Espera 45 segundos
               Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>Fin Parkings");
               Thread.Sleep(45000);
            }
        }

        public void obtenerDeusto()
        {
            while (true)
            {
                Thread.Sleep(5000);
                dinamico.parkingDeusto();
                InformacionEstatica.emisor.enviarParkingDeusto(dinamico.contadorDBSDeusto, dinamico.contadorGeneralDeusto, dinamico.descargaDeusto);

                //Espera un Minuto
                Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>Fin Deusto");
                Thread.Sleep(60000);
            }
        }

        public void obtenerBilbobus()
        {
            while (true)
            {
                Thread.Sleep(5000);
                estatico.paradasAutobusesBilbo();
                dinamico.tiemposParadaBilbo(estatico);
                estatico.lineasBilbobus();
                //InformacionEstatica.emisor.enviarTiemposParadas(estatico.paradasBilbobus, dinamico.descargaBilbobus);
                InformacionEstatica.emisor.enviarTiemposLineas(estatico.lineasBilbo, dinamico.descargaBilbobus);

                //Espera 45 segundos
                Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>Fin Bilbobus");
                Thread.Sleep(45000);
            }
        }

        public void obtenerTiempoBilbao()
        {
            while (true)
            {
                Thread.Sleep(5000);
                dinamico.meteorologiaCiudad();
                Dictionary<string, TiempoDiaCiudad> temporal = new Dictionary<string, TiempoDiaCiudad>();
                Dictionary<string, string> descripcionesES = new Dictionary<string, string>();
                Dictionary<string, string> descripcionesEU = new Dictionary<string, string>();
                temporal.Add("Hoy",dinamico.tiempoPorCiudades["Hoy"].tiempoCiudades["Bilbao"]);
                descripcionesES.Add("Hoy", dinamico.tiempoPorCiudades["Hoy"].descripcionES);
                descripcionesEU.Add("Hoy", dinamico.tiempoPorCiudades["Hoy"].descripcionEU);
                temporal.Add("Mañana", dinamico.tiempoPorCiudades["Mañana"].tiempoCiudades["Bilbao"]);
                descripcionesES.Add("Mañana", dinamico.tiempoPorCiudades["Mañana"].descripcionES);
                descripcionesEU.Add("Mañana", dinamico.tiempoPorCiudades["Mañana"].descripcionEU);
                temporal.Add("Pasado", dinamico.tiempoPorCiudades["Pasado"].tiempoCiudades["Bilbao"]);
                descripcionesES.Add("Pasado", dinamico.tiempoPorCiudades["Pasado"].descripcionES);
                descripcionesEU.Add("Pasado", dinamico.tiempoPorCiudades["Pasado"].descripcionEU);
                InformacionEstatica.emisor.enviarTiempoBilbao(temporal, descripcionesES, descripcionesEU, "Bilbao");

                //Espera un día
                Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>Fin Meteo");
                Thread.Sleep(86400000);
            }
        }

    }
}
