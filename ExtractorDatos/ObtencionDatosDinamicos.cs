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

            incidencias.Start();
            obtenerBicis.Start();
            obtenerParkings.Start();
            deusto.Start();
            obtenerBilbo.Start();
        }

        public void obtenerIncidencias()
        {
            while (true)
            {
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
                Thread.Sleep(86400000);
            }
            
        }

        public void obtenerBicis()
        {
            while (true)
            {
                dinamico.bicicletas();
                InformacionEstatica.emisor.enviarBicicletas(dinamico.puntosBicisList, dinamico.descargaBicis);

                //Espera un Minuto
                Thread.Sleep(60000);
            }
        }

        public void obtenerParkings()
        {
            while (true)
            {
               estatico.obtenerInformacionParkings();
               InformacionEstatica.emisor.enviarParkings(estatico.parkings, estatico.descargaParkings);
                
               //Espera un Minuto
               Thread.Sleep(60000);
            }
        }

        public void obtenerDeusto()
        {
            while (true)
            {
                dinamico.parkingDeusto();
                InformacionEstatica.emisor.enviarParkingDeusto(dinamico.contadorDBSDeusto, dinamico.contadorGeneralDeusto, dinamico.descargaDeusto);

                //Espera un Minuto
                Thread.Sleep(60000);
            }
        }

        public void obtenerBilbobus()
        {
            while (true)
            {
                estatico.paradasAutobusesBilbo();
                dinamico.tiemposParadaBilbo(estatico);
                estatico.lineasBilbobus();
                InformacionEstatica.emisor.enviarTiemposParadas(estatico.paradasBilbobus, dinamico.descargaBilbobus);
                InformacionEstatica.emisor.enviarTiemposLineas(estatico.lineasBilbo, dinamico.descargaBilbobus);

                //Espera un Minuto
                Thread.Sleep(60000);
            }
        }

    }
}
