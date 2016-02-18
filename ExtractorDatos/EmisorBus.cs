using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.ActiveMQ.Commands;
using Clases;
using Newtonsoft.Json.Linq;
using WrappersActiveMQ;

namespace ExtractorDatos
{
    internal class EmisorBus
    {
        private IConnection _connection;
        private ISession _session;
        private const String QUEUE_DESTINATION = "PruebaEMISOR";
        private IMessageProducer _producer;

        private DateTime inicio;
        private DateTime fin;
        private int cpIncidencia;

        public void inicializar()
        {
            // configure the broker
            try
            {
                // Create a ConnectionFactory
                IConnectionFactory connectionFactory = new ConnectionFactory(
                    "tcp://localhost:61616?jms.prefetchPolicy.all=1");

                // Create a Connection
                _connection = connectionFactory.CreateConnection();
                _connection.Start();

                // Create a Session
                _session = _connection.CreateSession();

                // Create the destination (Topic or Queue)
                IDestination destination = _session.GetTopic(QUEUE_DESTINATION);

                // Create a MessageProducer from the Session to the Topic or Queue
                _producer = _session.CreateProducer(destination);


            }
            catch (Exception e)
            {
                // TODO Auto-generated catch block
                Console.WriteLine(e.Message);
            }

        }

        public void limpiar()
        {
            _session.Close();
            _connection.Close();
        }

        public void enviarMensaje(string text)
        {
            ITextMessage objecto = _producer.CreateTextMessage(text);
            _producer.Send(objecto);
        }

        public void enviarParkings(Dictionary<int, Parking> m, DateTime descarga)
        {
            foreach (Parking parking in m.Values)
            {
                if (parking.entradas != null && parking.entradas.Count > 0)
                {
                    int cp = 00000;
                    if (parking.latlong.latitud > 0 && parking.latlong.longitud < 0)
                    {
                        string longitudS = ("" + parking.latlong.longitud).Replace(',', '.');
                        string latitudS = ("" + parking.latlong.latitud).Replace(',', '.');

                        string url = "http://geocode.arcgis.com/arcgis/rest/services/World/GeocodeServer/reverseGeocode?location=" + longitudS + "%2C" + latitudS + "&distance=200&outSR=&f=pjson";
                        string json = "";
                        using (WebClient wc = new WebClient())
                        {
                            json = wc.DownloadString(url);
                        }

                        JToken obj = JObject.Parse(json);
                        JToken direccion = obj.SelectToken("address");
                        cp = int.Parse(direccion.SelectToken("Postal").Value<string>());
                    }
                    XElement xml = convertirUnParking(parking, descarga, cp);
                    ITextMessage temporal = _producer.CreateXmlMessage(xml);
                    temporal.Properties.SetInt("CP", cp);
                    temporal.NMSType = "Parkings";
                    temporal.NMSTimeToLive = TimeSpan.FromMilliseconds(60000);
                    _producer.Send(temporal);
                }
            }
        }

        public void enviarTiemposParadas(Dictionary<int, ParadaBilbo> m, DateTime descarga)
        {
            foreach (ParadaBilbo paradaBilbo in m.Values)
            {
                string longitudS = ("" + paradaBilbo.lugar.longitud).Replace(',', '.');
                string latitudS = ("" + paradaBilbo.lugar.latitud).Replace(',', '.');

                string url = "http://geocode.arcgis.com/arcgis/rest/services/World/GeocodeServer/reverseGeocode?location=" + longitudS + "%2C" + latitudS + "&distance=200&outSR=&f=pjson";
                string json = "";
                using (WebClient wc = new WebClient())
                {
                    json = wc.DownloadString(url);
                }

                JToken obj = JObject.Parse(json);
                JToken direccion = obj.SelectToken("address");
                int cp = 0;
                try
                {
                    cp = int.Parse(direccion.SelectToken("Postal").Value<string>());
                }
                catch (NullReferenceException ex)
                {
                    Console.WriteLine("Se desconoce el código postal de " + longitudS + "/" + latitudS);
                }

                XElement xml = convertirTiempoDeUnaParada(paradaBilbo, descarga, cp);
                ITextMessage temporal = _producer.CreateXmlMessage(xml);
                temporal.Properties.SetInt("CP", cp);
                temporal.NMSType = "TiemposParada";
                temporal.NMSTimeToLive = TimeSpan.FromMilliseconds(60000);
                _producer.Send(temporal);
            }
        }

        public void enviarTiemposLineas(Dictionary<string, LineaBilbobus> lineas, DateTime descarga)
        {
            foreach (LineaBilbobus lineaBilbobus in lineas.Values)
            {
                XElement xml = convertirTiempoDeUnaLinea(lineaBilbobus, descarga);
                ITextMessage temporal = _producer.CreateXmlMessage(xml);
                temporal.NMSType = "TiemposLinea";
                temporal.NMSTimeToLive = TimeSpan.FromMilliseconds(60000);
                _producer.Send(temporal);
            }
        }

        public void enviarBicicletas(List<PuntoBici> bicis, DateTime descarga)
        {
            foreach (PuntoBici puntoBici in bicis)
            {
                string longitudS = ("" + puntoBici.localizacion.longitud).Replace(',', '.');
                string latitudS = ("" + puntoBici.localizacion.latitud).Replace(',', '.');

                string url = "http://geocode.arcgis.com/arcgis/rest/services/World/GeocodeServer/reverseGeocode?location=" + longitudS + "%2C" + latitudS + "&distance=200&outSR=&f=pjson";
                string json = "";
                using (WebClient wc = new WebClient())
                {
                    json = wc.DownloadString(url);
                }

                JToken obj = JObject.Parse(json);
                JToken direccion = obj.SelectToken("address");
                int cp = int.Parse(direccion.SelectToken("Postal").Value<string>());

                XElement xml = convertirUnPuntoBici(puntoBici, descarga, cp);
                ITextMessage temporal = _producer.CreateXmlMessage(xml);
                temporal.Properties.SetInt("CP", cp);
                temporal.NMSType = "Bicis";
                temporal.NMSTimeToLive = TimeSpan.FromMilliseconds(60000);
                _producer.Send(temporal);
            }
        }

        public void enviarParkingDeusto(int dbs, int general, DateTime descarga)
        {
            XElement xml = convertirParkingDeusto(dbs, general, descarga, 48014);
            ITextMessage temporal = _producer.CreateXmlMessage(xml);
            temporal.Properties.SetInt("CP", 48014);
            temporal.NMSType = "Deusto";
            temporal.NMSTimeToLive = TimeSpan.FromMilliseconds(60000);
            _producer.Send(temporal);
        }

        public void enviarIncidencias<U, T>(U arg, DateTime descarga) where U : IEnumerable<T>
        {
            foreach (T variable in arg)
            {
                XElement xml = convertirIncidencia(variable, descarga);
                ITextMessage temporal = _producer.CreateXmlMessage(xml);
                temporal.Properties.SetInt("CP", cpIncidencia);
                temporal.NMSType = "Incidencia";
                temporal.NMSTimeToLive = TimeSpan.FromMilliseconds(86400000);
                _producer.Send(temporal);
            }
        }

        public void enviarTiempoBilbao(Dictionary<string, TiempoDiaCiudad> tiempos, Dictionary<string, string> es, Dictionary<string, string> eu, string ciudad)
        {
            XElement xml = convertirTiempoCiudad(tiempos, ciudad, es, eu);
            ITextMessage temporal = _producer.CreateXmlMessage(xml);
            temporal.NMSType = "TiempoCiudad";
            temporal.NMSTimeToLive = TimeSpan.FromMilliseconds(86400000);
            _producer.Send(temporal);
        }

/// <summary>
/// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// </summary>
/// <param name="tiempoC"></param>
/// 
        public void enviarTiempoCiudad(Dictionary<string, TiempoCiudad> tiempoC)
        {
            try
            {
                List<System.Collections.Generic.KeyValuePair<string, TiempoCiudad>> temporal = tiempoC.ToList();
                List<Clases.KeyValuePair<string, TiempoCiudad>> miKeyValue = new List<Clases.KeyValuePair<string, TiempoCiudad>>();
                foreach (System.Collections.Generic.KeyValuePair<string, TiempoCiudad> valuePair in temporal)
                {
                    miKeyValue.Add(new Clases.KeyValuePair<string, TiempoCiudad>(valuePair.Key, valuePair.Value));
                }
                foreach (Clases.KeyValuePair<string, TiempoCiudad> pair in miKeyValue)
                {
                    Console.WriteLine(pair.Value.descripcionES);
                }
                _producer.Send(new ParesTiempoCiudad(miKeyValue));
            }
            catch (System.NullReferenceException ex)
            {
                Console.WriteLine("No se ha podido inicializar el productor. Compruebe que ApacheMQ esté encendido.");
                Console.ReadKey();
            }
        }

        public void enviarTiempoComarca(Dictionary<string, TiempoComarca> tiempoC)
        {
            try
            {
                List<System.Collections.Generic.KeyValuePair<string, TiempoComarca>> temporal = tiempoC.ToList();
                List<Clases.KeyValuePair<string, TiempoComarca>> miKeyValue = new List<Clases.KeyValuePair<string, TiempoComarca>>();
                foreach (System.Collections.Generic.KeyValuePair<string, TiempoComarca> valuePair in temporal)
                {
                    miKeyValue.Add(new Clases.KeyValuePair<string, TiempoComarca>(valuePair.Key, valuePair.Value));
                }
                foreach (Clases.KeyValuePair<string, TiempoComarca> pair in miKeyValue)
                {
                    Console.WriteLine(pair.Value.nombreComarcaES);
                }
                _producer.Send(new ParesTiempoComarca(miKeyValue));
            }
            catch (System.NullReferenceException ex)
            {
                Console.WriteLine("No se ha podido inicializar el productor. Compruebe que ApacheMQ esté encendido.");
                Console.ReadKey();
            }
        }

        public void enviarTiempoPrediccion(TiempoPrediccion tiempoP)
        {
            try
            {
                _producer.Send(tiempoP);
            }
            catch (System.NullReferenceException ex)
            {
                Console.WriteLine("No se ha podido inicializar el productor. Compruebe que ApacheMQ esté encendido.");
                Console.ReadKey();
            }
        }

        public void enviarEventosTrafico(List<Evento>eventos)
        {
            try
            {
                _producer.Send(new WrapperEvento(eventos));
            }
            catch (System.NullReferenceException ex)
            {
                Console.WriteLine("No se ha podido inicializar el productor. Compruebe que ApacheMQ esté encendido.");
                Console.ReadKey();
            }
        }

        public void enviarObrasTrafico(List<Obra> obras)
        {
            try
            {
                _producer.Send(new WrapperObra(obras));
            }
            catch (System.NullReferenceException ex)
            {
                Console.WriteLine("No se ha podido inicializar el productor. Compruebe que ApacheMQ esté encendido.");
                Console.ReadKey();
            }
        }

        public void enviarIncidenciasTrafico(List<Incidencia> incidencias)
        {
            try
            {
                _producer.Send(new WrapperIncidencia(incidencias));
            }
            catch (System.NullReferenceException ex)
            {
                Console.WriteLine("No se ha podido inicializar el productor. Compruebe que ApacheMQ esté encendido.");
                Console.ReadKey();
            }
        }

        public void enviarMantenimientosTrafico(List<Mantenimiento> mantenimientos)
        {
            try
            {
                _producer.Send(new WrapperMantenimiento(mantenimientos));
            }
            catch (System.NullReferenceException ex)
            {
                Console.WriteLine("No se ha podido inicializar el productor. Compruebe que ApacheMQ esté encendido.");
                Console.ReadKey();
            }
        }

        public void enviarParadasEuskotren(Dictionary<int, ParadaEuskotren> m)
        {
            //IObjectMessage objecto = _producer.CreateObjectMessage(m);
            try
            {
                List<System.Collections.Generic.KeyValuePair<int, ParadaEuskotren>> temporal = m.ToList();
                List<Clases.KeyValuePair<int, ParadaEuskotren>> miKeyValue = new List<Clases.KeyValuePair<int, ParadaEuskotren>>();
                foreach (System.Collections.Generic.KeyValuePair<int, ParadaEuskotren> valuePair in temporal)
                {
                    miKeyValue.Add(new Clases.KeyValuePair<int, ParadaEuskotren>(valuePair.Key, valuePair.Value));
                }
                _producer.Send(new ParesEuskotren(miKeyValue));
            }
            catch (System.NullReferenceException ex)
            {
                Console.WriteLine("No se ha podido inicializar el productor. Compruebe que ApacheMQ esté encendido.");
                Console.ReadKey();
            }

        }

        public void enviarParadasBizkaibus(Dictionary<int, ParadaBizkaibus> m)
        {
            //IObjectMessage objecto = _producer.CreateObjectMessage(m);
            try
            {
                List<System.Collections.Generic.KeyValuePair<int, ParadaBizkaibus>> temporal = m.ToList();
                List<Clases.KeyValuePair<int, ParadaBizkaibus>> miKeyValue = new List<Clases.KeyValuePair<int, ParadaBizkaibus>>();
                foreach (System.Collections.Generic.KeyValuePair<int, ParadaBizkaibus> valuePair in temporal)
                {
                    miKeyValue.Add(new Clases.KeyValuePair<int, ParadaBizkaibus>(valuePair.Key, valuePair.Value));
                }
                _producer.Send(new ParesBizkaibus(miKeyValue));
            }
            catch (System.NullReferenceException ex)
            {
                Console.WriteLine("No se ha podido inicializar el productor. Compruebe que ApacheMQ esté encendido.");
                Console.ReadKey();
            }

        }

        public void enviarFarmacias(List<Farmacia> farmacias)
        {
            try
            {
                _producer.Send(new WrapperFarmacia(farmacias));
            }
            catch (System.NullReferenceException ex)
            {
                Console.WriteLine("No se ha podido inicializar el productor. Compruebe que ApacheMQ esté encendido.");
                Console.ReadKey();
            }
        }

        public void enviarHospitales(List<Hospital> hospitales)
        {
            try
            {
                _producer.Send(new WrapperHospital(hospitales));
            }
            catch (System.NullReferenceException ex)
            {
                Console.WriteLine("No se ha podido inicializar el productor. Compruebe que ApacheMQ esté encendido.");
                Console.ReadKey();
            }
        }

        public void enviarCentrosSalud(List<CentroSalud> centrosSalud)
        {
            try
            {
                _producer.Send(new WrapperCentroSalud(centrosSalud));
            }
            catch (System.NullReferenceException ex)
            {
                Console.WriteLine("No se ha podido inicializar el productor. Compruebe que ApacheMQ esté encendido.");
                Console.ReadKey();
            }
        }

        public void enviarLineasBilbobus(Dictionary<string, LineaBilbobus> lineas)
        {
            //IObjectMessage objecto = _producer.CreateObjectMessage(m);
            try
            {
                List<System.Collections.Generic.KeyValuePair<string, LineaBilbobus>> temporal = lineas.ToList();
                List<Clases.KeyValuePair<string, LineaBilbobus>> miKeyValue = new List<Clases.KeyValuePair<string, LineaBilbobus>>();
                foreach (System.Collections.Generic.KeyValuePair<string, LineaBilbobus> valuePair in temporal)
                {
                    foreach (Clases.KeyValuePair<int, ViajeBilbobus> pair in valuePair.Value.viajes)
                    {
                        pair.Value.paradas = null;
                    }
                    miKeyValue.Add(new Clases.KeyValuePair<string, LineaBilbobus>(valuePair.Key, valuePair.Value));
                }
                _producer.Send(new ParesLineasBilbobus(miKeyValue));
            }
            catch (System.NullReferenceException ex)
            {
                Console.WriteLine("No se ha podido inicializar el productor. Compruebe que ApacheMQ esté encendido.");
                Console.ReadKey();
            }

        }

        public void enviarLineasEuskotren(Dictionary<int, LineaEuskotren> lineas)
        {
            //IObjectMessage objecto = _producer.CreateObjectMessage(m);
            try
            {
                List<System.Collections.Generic.KeyValuePair<int, LineaEuskotren>> temporal = lineas.ToList();
                List<Clases.KeyValuePair<int, LineaEuskotren>> miKeyValue = new List<Clases.KeyValuePair<int, LineaEuskotren>>();
                foreach (System.Collections.Generic.KeyValuePair<int, LineaEuskotren> valuePair in temporal)
                {
                    foreach (Clases.KeyValuePair<string, ViajeEuskotren> pair in valuePair.Value.viajes)
                    {
                        pair.Value.paradas = null;
                    }
                    miKeyValue.Add(new Clases.KeyValuePair<int, LineaEuskotren>(valuePair.Key, valuePair.Value));
                }
                _producer.Send(new ParesLineasEuskotren(miKeyValue));
            }
            catch (System.NullReferenceException ex)
            {
                Console.WriteLine("No se ha podido inicializar el productor. Compruebe que ApacheMQ esté encendido.");
                Console.ReadKey();
            }

        }

        public void enviarLineasBizkaibus(Dictionary<int, LineaBizkaibus> lineas)
        {
            //IObjectMessage objecto = _producer.CreateObjectMessage(m);
            try
            {
                List<System.Collections.Generic.KeyValuePair<int, LineaBizkaibus>> temporal = lineas.ToList();
                List<Clases.KeyValuePair<int, LineaBizkaibus>> miKeyValue = new List<Clases.KeyValuePair<int, LineaBizkaibus>>();
                foreach (System.Collections.Generic.KeyValuePair<int, LineaBizkaibus> valuePair in temporal)
                {
                    foreach (Clases.KeyValuePair<int, ViajeBizkaibus> pair in valuePair.Value.viajes)
                    {
                        pair.Value.paradas = null;
                    }
                    miKeyValue.Add(new Clases.KeyValuePair<int, LineaBizkaibus>(valuePair.Key, valuePair.Value));
                }
                _producer.Send(new ParesLineasBizkaibus(miKeyValue));
            }
            catch (System.NullReferenceException ex)
            {
                Console.WriteLine("No se ha podido inicializar el productor. Compruebe que ApacheMQ esté encendido.");
                Console.ReadKey();
            }

        }
        public void enviarLineasMetro(Dictionary<string, LineaMetro> metro)
        {
            //IObjectMessage objecto = _producer.CreateObjectMessage(m);
            try
            {
                List<System.Collections.Generic.KeyValuePair<string, LineaMetro>> temporal = metro.ToList();
                List<Clases.KeyValuePair<string, LineaMetro>> miKeyValue = new List<Clases.KeyValuePair<string, LineaMetro>>();
                foreach (System.Collections.Generic.KeyValuePair<string, LineaMetro> valuePair in temporal)
                {
                    miKeyValue.Add(new Clases.KeyValuePair<string, LineaMetro>(valuePair.Key, valuePair.Value));
                }
                _producer.Send(new ParesLineaMetro(miKeyValue));
            }
            catch (System.NullReferenceException ex)
            {
                Console.WriteLine("No se ha podido inicializar el productor. Compruebe que ApacheMQ esté encendido.");
                Console.ReadKey();
            }

        }

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Generadores de XML
        public XElement convertirXMLParking(Dictionary<int, Parking> parkings, DateTime descarga)
        {

            XElement coleccionParkings = new XElement("Parkings");
            //Cabecera

            XElement cabecera = new XElement("Cabecera");
            XElement tipo = new XElement("Tipo", "parkingId");
            XElement zona = new XElement("Zona", "ZonaX");
            XElement fecha = new XElement("FechaDescarga", descarga);

            XElement influencia = new XElement("Influencia");
            XElement codigoPostal = new XElement("CP", 48001);
            XElement barrio = new XElement("Barrio", "Desconocido");
            XElement distrito = new XElement("Distrito", "Desconocido");
            influencia.Add(codigoPostal);
            influencia.Add(barrio);
            influencia.Add(distrito);
                
            cabecera.Add(tipo);
            cabecera.Add(zona);
            cabecera.Add(influencia);
            cabecera.Add(fecha);

            coleccionParkings.Add(cabecera);

            //Contenido

            XElement cuerpo = new XElement("Cuerpo");
            foreach (System.Collections.Generic.KeyValuePair<int, Parking> temp in parkings.OrderBy(i => i.Key))
            {
                Parking objParking = temp.Value;
                if (objParking.tipo.Equals("Subterraneo") && !objParking.estado.Equals("closed"))
                {
                    XElement parking = new XElement("Parking", new XAttribute("id", objParking.id));

                    XElement id = new XElement("Id", objParking.id);
                    XElement nombreParking = new XElement("Nombre", objParking.nombre);

                    XElement localizacion = new XElement("Localizacion");
                    XElement latitud = new XElement("Latitud", objParking.latlong.latitud);
                    XElement longitud = new XElement("Longitud", objParking.latlong.longitud);
                    localizacion.Add(latitud);
                    localizacion.Add(longitud);

                    XElement capacidad = new XElement("Capacidad", objParking.capacidad);
                   // Console.WriteLine(objParking.capacidad + " " + objParking.ocupacion);
                    XElement disponibilidad = new XElement("Disponibilidad", (objParking.capacidad - objParking.ocupacion));

                    parking.Add(id);
                    parking.Add(nombreParking);
                    parking.Add(localizacion);
                    parking.Add(capacidad);
                    parking.Add(disponibilidad);

                    cuerpo.Add(parking);
                }

            }
            coleccionParkings.Add(cuerpo);

            Console.WriteLine(coleccionParkings.ToString());
            return coleccionParkings;
        }

        public XElement convertirXMLBicis(List<PuntoBici> bicis)
        {
            XElement coleccionBicis = new XElement("PuntosBicis");
            //Cabecera

            XElement cabecera = new XElement("Cabecera");
            XElement tipo = new XElement("Tipo", "PBB");
            XElement zona = new XElement("Zona", "ZonaX");

            XElement influencia = new XElement("Influencia");
            XElement codigoPostal = new XElement("CP", 48001);
            XElement barrio = new XElement("Barrio", "Desconocido");
            XElement distrito = new XElement("Distrito", "Desconocido");
            influencia.Add(codigoPostal);
            influencia.Add(barrio);
            influencia.Add(distrito);

            cabecera.Add(tipo);
            cabecera.Add(zona);
            cabecera.Add(influencia);

            coleccionBicis.Add(cabecera);

            //Contenido

            XElement cuerpo = new XElement("Cuerpo");
            foreach (PuntoBici temp in bicis)
            {
                XElement bici = new XElement("PuntoBici", new XAttribute("id", temp.id));

                XElement id = new XElement("Id", temp.id);
                XElement nombrePunto = new XElement("Nombre", temp.nombre);

                XElement localizacion = new XElement("Localizacion");
                XElement latitud = new XElement("Latitud", temp.localizacion.latitud);
                XElement longitud = new XElement("Longitud", temp.localizacion.longitud);
                localizacion.Add(latitud);
                localizacion.Add(longitud);

                XElement capacidad = new XElement("CapacidadAnclaje", temp.anclajesLibres+temp.anclajesAveriados+temp.anclajesUsados);
                // Console.WriteLine(objParking.capacidad + " " + objParking.ocupacion);
                XElement bicislibres = new XElement("BicisLibres", temp.bicisLibres);
                XElement disponibilidad = new XElement("DisponibilidadAnclaje", temp.anclajesLibres);

                bici.Add(id);
                bici.Add(nombrePunto);
                bici.Add(localizacion);
                bici.Add(capacidad);
                bici.Add(bicislibres);
                bici.Add(disponibilidad);

                cuerpo.Add(bici);
                
            }

            coleccionBicis.Add(cuerpo);
            Console.WriteLine(coleccionBicis.ToString());
            return coleccionBicis;
        }

        public XElement convertirXMLTiemposParada(Dictionary<int, ParadaBilbo> paradas)
        {
            XElement coleccionParadas = new XElement("TiemposParada");
            //Cabecera

            XElement cabecera = new XElement("Cabecera");
            XElement tipo = new XElement("Tipo", "paradaId");
            XElement zona = new XElement("Zona", "ZonaX");

            XElement influencia = new XElement("Influencia");
            XElement codigoPostal = new XElement("CP", 48001);
            XElement barrio = new XElement("Barrio", "Desconocido");
            XElement distrito = new XElement("Distrito", "Desconocido");
            influencia.Add(codigoPostal);
            influencia.Add(barrio);
            influencia.Add(distrito);

            cabecera.Add(tipo);
            cabecera.Add(zona);
            cabecera.Add(influencia);

            coleccionParadas.Add(cabecera);

            //Contenido

            XElement cuerpo = new XElement("Cuerpo");
            foreach (ParadaBilbo temp in paradas.Values)
            {
                XElement parada = new XElement("TiempoParada", new XAttribute("id", temp.idParada));

                XElement id = new XElement("Id", temp.idParada);
                XElement nombrePunto = new XElement("Nombre", temp.nombre);

                XElement localizacion = new XElement("Localizacion");
                XElement latitud = new XElement("Latitud", temp.lugar.latitud);
                XElement longitud = new XElement("Longitud", temp.lugar.longitud);
                localizacion.Add(latitud);
                localizacion.Add(longitud);

                XElement lineas = new XElement("Lineas");
                foreach (LineaBusTiempo var in temp.lineasYTiempo.Values)
                {
                    XElement linea = new XElement("Linea", new XAttribute("id", var.codigoLinea));
                    XElement idLinea = new XElement("Id", var.codigoLinea);
                    XElement nombreLinea = new XElement("NombreLinea", var.descripcionLinea);
                    XElement tiempo = new XElement("TiempoRestante", var.tiempoEspera);
                    

                    linea.Add(idLinea);
                    linea.Add(nombreLinea);
                    linea.Add(tiempo);

                    lineas.Add(linea);
                }
                if (!lineas.IsEmpty)
                {
                    Console.WriteLine(parada.ToString() + "" + lineas.ToString());
                }
                

                parada.Add(id);
                parada.Add(nombrePunto);
                parada.Add(localizacion);
                parada.Add(lineas);

                cuerpo.Add(parada);


            }
            coleccionParadas.Add(cuerpo);

            //Console.WriteLine(coleccionParadas.ToString());
            return coleccionParadas;
        }

        public XElement convertirXMLTiemposLinea(Dictionary<string, LineaBilbobus> lineasBilbo)
        {
            XElement coleccionLineas = new XElement("TiemposLinea");
            //Cabecera

            XElement cabecera = new XElement("Cabecera");
            XElement tipo = new XElement("Tipo", "lineaId");
            XElement zona = new XElement("Zona", "ZonaX");

            XElement influencia = new XElement("Influencia");
            XElement codigoPostal = new XElement("CP", 48001);
            XElement barrio = new XElement("Barrio", "Desconocido");
            XElement distrito = new XElement("Distrito", "Desconocido");
            influencia.Add(codigoPostal);
            influencia.Add(barrio);
            influencia.Add(distrito);

            cabecera.Add(tipo);
            cabecera.Add(zona);
            cabecera.Add(influencia);

            coleccionLineas.Add(cabecera);

            //Contenido

            XElement cuerpo = new XElement("Cuerpo");
            Console.WriteLine(lineasBilbo.Values.Count);
            foreach (LineaBilbobus temp in lineasBilbo.Values)
            {
                XElement nodoLinea = new XElement("Linea", new XAttribute("id", temp.id));
                XElement paradas = new XElement("Paradas");

                
                if (temp.viajes.Count > 0)
                {
                    Clases.KeyValuePair<int, ViajeBilbobus> viajeLinea = temp.viajes[0];
                    List<Clases.KeyValuePair<int, ParadaBilbo>> paradasViaje = viajeLinea.Value.paradas;

                    foreach (var paradasDeViaje in paradasViaje)
                    {
                        ParadaBilbo objParada = paradasDeViaje.Value;
                        XElement parada = new XElement("Parada", new XAttribute("id", objParada.idParada));

                        XElement idParada = new XElement("Id", objParada.idParada);
                        XElement nombreParada = new XElement("Nombre", objParada.nombre);

                        XElement localizacionParada = new XElement("Localizacion");
                        XElement latitudP = new XElement("Latitud", objParada.lugar.latitud);
                        XElement longitudP = new XElement("Longitud", objParada.lugar.longitud);
                        localizacionParada.Add(latitudP);
                        localizacionParada.Add(longitudP);

                        
                        XElement tiempoRestante = null;
                        if (!objParada.lineasYTiempo.ContainsKey(temp.id))
                        {
                            tiempoRestante = new XElement("TiempoRestante", -1);
                        }
                        else
                        {
                            Console.WriteLine(objParada.lineasYTiempo[temp.id].tiempoEspera);
                            tiempoRestante = new XElement("TiempoRestante", objParada.lineasYTiempo[temp.id].tiempoEspera);
                        }

                        parada.Add(idParada);
                        parada.Add(nombreParada);
                        parada.Add(localizacionParada);
                        parada.Add(tiempoRestante);

                        paradas.Add(parada);
                       // Console.WriteLine(paradas.ToString());

                    }
                }
                
                nodoLinea.Add(paradas);
                cuerpo.Add(nodoLinea);
            }
            coleccionLineas.Add(cuerpo);

            //Console.WriteLine(coleccionLineas.ToString());
            return coleccionLineas;
        }

        public XElement convertirFuturosUsos()
        {
            XElement futuro = new XElement("TiemposLinea");
            //Cabecera

            XElement cabecera = new XElement("Cabecera");
            XElement tipo = new XElement("Tipo", "TBD");
            XElement zona = new XElement("Zona", "ZonaX");

            XElement influencia = new XElement("Influencia");
            XElement codigoPostal = new XElement("CP", 48001);
            XElement barrio = new XElement("Barrio", "Desconocido");
            XElement distrito = new XElement("Distrito", "Desconocido");
            influencia.Add(codigoPostal);
            influencia.Add(barrio);
            influencia.Add(distrito);

            cabecera.Add(tipo);
            cabecera.Add(zona);
            cabecera.Add(influencia);

            futuro.Add(cabecera);



            return futuro;
        }

        public XElement convertirUnParking(Parking p, DateTime descarga, int cp)
        {
            XElement xmlParking = new XElement("Parking", new XAttribute("id", p.id));
            //Cabecera

            XElement cabecera = new XElement("Cabecera");
            XElement tipo = new XElement("Tipo", "parkingId");
            XElement zona = new XElement("Zona", "ZonaX");
            XElement fecha = new XElement("FechaDescarga", descarga);

            XElement influencia = new XElement("Influencia");
            XElement codigoPostal = new XElement("CP", cp);
            XElement barrio = new XElement("Barrio", "Desconocido");
            XElement distrito = new XElement("Distrito", "Desconocido");
            influencia.Add(codigoPostal);
            influencia.Add(barrio);
            influencia.Add(distrito);

            cabecera.Add(tipo);
            cabecera.Add(zona);
            cabecera.Add(influencia);
            cabecera.Add(fecha);

            xmlParking.Add(cabecera);

            //Contenido

            XElement cuerpo = new XElement("Cuerpo");

            XElement id = new XElement("Id", p.id);
            XElement nombreParking = new XElement("Nombre", p.nombre);

            XElement localizacion = new XElement("Localizacion");
            XElement latitud = new XElement("Latitud", p.latlong.latitud);
            XElement longitud = new XElement("Longitud", p.latlong.longitud);
            localizacion.Add(latitud);
            localizacion.Add(longitud);

            XElement capacidad = new XElement("Capacidad", p.capacidad);
            // Console.WriteLine(objParking.capacidad + " " + objParking.ocupacion);
            XElement disponibilidad = new XElement("Disponibilidad", (p.capacidad - p.ocupacion));

            cuerpo.Add(id);
            cuerpo.Add(nombreParking);
            cuerpo.Add(localizacion);
            cuerpo.Add(capacidad);
            cuerpo.Add(disponibilidad);

            xmlParking.Add(cuerpo);

            Console.WriteLine(xmlParking.ToString());
            return xmlParking;
        }

        public XElement convertirUnPuntoBici(PuntoBici pb, DateTime descarga, int cp)
        {
            XElement puntoBici = new XElement("PuntoBici", new XAttribute("id", pb.id));
            //Cabecera

            XElement cabecera = new XElement("Cabecera");
            XElement tipo = new XElement("Tipo", "PBB");
            XElement zona = new XElement("Zona", "ZonaX");
            XElement fecha = new XElement("FechaDescarga", descarga);

            XElement influencia = new XElement("Influencia");
            XElement codigoPostal = new XElement("CP", cp);
            XElement barrio = new XElement("Barrio", "Desconocido");
            XElement distrito = new XElement("Distrito", "Desconocido");

            influencia.Add(codigoPostal);
            influencia.Add(barrio);
            influencia.Add(distrito);

            cabecera.Add(tipo);
            cabecera.Add(zona);
            cabecera.Add(influencia);
            cabecera.Add(fecha);

            puntoBici.Add(cabecera);

            //Contenido

            XElement cuerpo = new XElement("Cuerpo");

            XElement id = new XElement("Id", pb.id);
            XElement nombrePunto = new XElement("Nombre", pb.nombre);

            XElement localizacion = new XElement("Localizacion");
            XElement latitud = new XElement("Latitud", pb.localizacion.latitud);
            XElement longitud = new XElement("Longitud", pb.localizacion.longitud);
            localizacion.Add(latitud);
            localizacion.Add(longitud);

            XElement capacidad = new XElement("CapacidadAnclaje", pb.anclajesLibres + pb.anclajesAveriados + pb.anclajesUsados);
            // Console.WriteLine(objParking.capacidad + " " + objParking.ocupacion);
            XElement bicislibres = new XElement("BicisLibres", pb.bicisLibres);
            XElement disponibilidad = new XElement("DisponibilidadAnclaje", pb.anclajesLibres);
            XElement estado = new XElement("Estado", pb.estado);

            cuerpo.Add(id);
            cuerpo.Add(nombrePunto);
            cuerpo.Add(localizacion);
            cuerpo.Add(capacidad);
            cuerpo.Add(bicislibres);
            cuerpo.Add(disponibilidad);
            cuerpo.Add(estado);


            puntoBici.Add(cuerpo);
            Console.WriteLine(puntoBici.ToString());
            return puntoBici;
        }

        public XElement convertirTiempoDeUnaParada(ParadaBilbo pbi, DateTime descarga, int cp)
        {
            XElement parada = new XElement("TiemposParada", new XAttribute("id", pbi.id));
            //Cabecera

            XElement cabecera = new XElement("Cabecera");
            XElement tipo = new XElement("Tipo", "paradaId");
            XElement zona = new XElement("Zona", "ZonaX");
            XElement fecha = new XElement("FechaDescarga", descarga);

            XElement influencia = new XElement("Influencia");
            XElement codigoPostal = new XElement("CP", cp);
            XElement barrio = new XElement("Barrio", "Desconocido");
            XElement distrito = new XElement("Distrito", "Desconocido");
            influencia.Add(codigoPostal);
            influencia.Add(barrio);
            influencia.Add(distrito);

            cabecera.Add(tipo);
            cabecera.Add(zona);
            cabecera.Add(influencia);
            cabecera.Add(fecha);

            parada.Add(cabecera);

            //Contenido

            XElement cuerpo = new XElement("Cuerpo");

            XElement id = new XElement("Id", pbi.idParada);
            XElement nombrePunto = new XElement("Nombre", pbi.nombre);

            XElement localizacion = new XElement("Localizacion");
            XElement latitud = new XElement("Latitud", pbi.lugar.latitud);
            XElement longitud = new XElement("Longitud", pbi.lugar.longitud);
            localizacion.Add(latitud);
            localizacion.Add(longitud);

            XElement lineas = new XElement("Lineas");
            foreach (LineaBusTiempo var in pbi.lineasYTiempo.Values)
            {
                XElement linea = new XElement("Linea", new XAttribute("id", var.codigoLinea));
                XElement idLinea = new XElement("Id", var.codigoLinea);
                XElement nombreLinea = new XElement("NombreLinea", var.descripcionLinea);
                XElement tiempo = new XElement("TiempoRestante", var.tiempoEspera);


                linea.Add(idLinea);
                linea.Add(nombreLinea);
                linea.Add(tiempo);

                lineas.Add(linea);
            }
            if (!lineas.IsEmpty)
            {
                Console.WriteLine(pbi.ToString() + "" + lineas.ToString());
            }


            cuerpo.Add(id);
            cuerpo.Add(nombrePunto);    
            cuerpo.Add(localizacion);
            cuerpo.Add(lineas);

            parada.Add(cuerpo);

            //Console.WriteLine(coleccionParadas.ToString());
            return parada;
        }

        public XElement convertirTiempoDeUnaLinea(LineaBilbobus lb, DateTime descarga)
        {
            XElement linea = new XElement("TiemposLinea", new XAttribute("Id", lb.id));
            //Cabecera

            XElement cabecera = new XElement("Cabecera");
            XElement tipo = new XElement("Tipo", "lineaId");
            XElement zona = new XElement("Zona", "ZonaX");
            XElement fecha = new XElement("FechaDescarga", descarga);

            XElement influencia = new XElement("Influencia");
            XElement codigoPostal = new XElement("CP", 48001);
            XElement barrio = new XElement("Barrio", "Desconocido");
            XElement distrito = new XElement("Distrito", "Desconocido");
            influencia.Add(codigoPostal);
            influencia.Add(barrio);
            influencia.Add(distrito);

            cabecera.Add(tipo);
            cabecera.Add(zona);
            cabecera.Add(influencia);
            cabecera.Add(fecha);


            linea.Add(cabecera);

            //Contenido

            XElement cuerpo = new XElement("Cuerpo");
            XElement paradas = new XElement("Paradas");

            if (lb.viajes.Count > 0)
            {
                Clases.KeyValuePair<int, ViajeBilbobus> viajeLinea = lb.viajes[0];
                List<Clases.KeyValuePair<int, ParadaBilbo>> paradasViaje = viajeLinea.Value.paradas;

                foreach (var paradasDeViaje in paradasViaje)
                {
                    ParadaBilbo objParada = paradasDeViaje.Value;
                    XElement parada = new XElement("Parada", new XAttribute("id", objParada.idParada));

                    XElement idParada = new XElement("Id", objParada.idParada);
                    XElement nombreParada = new XElement("Nombre", objParada.nombre);

                    XElement localizacionParada = new XElement("Localizacion");
                    XElement latitudP = new XElement("Latitud", objParada.lugar.latitud);
                    XElement longitudP = new XElement("Longitud", objParada.lugar.longitud);
                    localizacionParada.Add(latitudP);
                    localizacionParada.Add(longitudP);

                    XElement tiempoRestante = null;
                    if (!objParada.lineasYTiempo.ContainsKey(lb.id))
                    {
                        tiempoRestante = new XElement("TiempoRestante", -1);
                    }
                    else
                    {
                        Console.WriteLine(objParada.lineasYTiempo[lb.id].tiempoEspera);
                        tiempoRestante = new XElement("TiempoRestante", objParada.lineasYTiempo[lb.id].tiempoEspera);
                    }

                    parada.Add(idParada);
                    parada.Add(nombreParada);
                    parada.Add(localizacionParada);
                    parada.Add(tiempoRestante);

                    paradas.Add(parada);
                    // Console.WriteLine(paradas.ToString());

                }
            }

            cuerpo.Add(paradas);
            linea.Add(cuerpo);

            //Console.WriteLine(coleccionLineas.ToString());
            return linea; 
        }

        public XElement convertirParkingDeusto(int dbs, int general, DateTime descarga, int cp)
        {
            XElement deusto = new XElement("ParkingDeusto");
            //Cabecera

            XElement cabecera = new XElement("Cabecera");
            XElement tipo = new XElement("Tipo", "parkingDeusto");
            XElement zona = new XElement("Zona", "ZonaX");
            XElement fecha = new XElement("FechaDescarga", descarga);

            XElement influencia = new XElement("Influencia");
            XElement codigoPostal = new XElement("CP", cp);
            XElement barrio = new XElement("Barrio", "Desconocido");
            XElement distrito = new XElement("Distrito", "Desconocido");
            influencia.Add(codigoPostal);
            influencia.Add(barrio);
            influencia.Add(distrito);

            cabecera.Add(tipo);
            cabecera.Add(zona);
            cabecera.Add(influencia);
            cabecera.Add(fecha);

            deusto.Add(cabecera);

            XElement cuerpo = new XElement("Cuerpo");
            XElement dbsX = new XElement("Dbs", dbs);
            XElement generalX = new XElement("General", general);

            cuerpo.Add(dbsX);
            cuerpo.Add(generalX);

            deusto.Add(cuerpo);

            return deusto;

        }

        public XElement convertirIncidencia<T>(T arg, DateTime descarga)
        {
            XElement incidencia = new XElement("Incidencia");
            //Cabecera

            XElement cabecera = new XElement("Cabecera");
            XElement tipo = new XElement("Tipo", "incidenciaTrafico");
            XElement zona = new XElement("Zona", "ZonaX");
            XElement fecha = new XElement("FechaDescarga", descarga);

            XElement influencia = new XElement("Influencia");
            XElement barrio = new XElement("Barrio", "Desconocido");
            XElement distrito = new XElement("Distrito", "Desconocido");
           

            XElement id = null;
            XElement fechaInicio = null;
            XElement fechaFin = null;
            XElement descripcion = null;
            XElement localizacion = null;
            XElement latitud = null;
            XElement longitud = null;
            XElement ambito = null;

            int cp = 0;
            string longitudS = "";
            string latitudS = "";

            if(arg is Evento)
            {
                longitudS = ("" + (arg as Evento).localizacion.longitud).Replace(',', '.');
                latitudS = ("" + (arg as Evento).localizacion.latitud).Replace(',', '.');

                ambito = new XElement("Ambito","Evento");
                id= new XElement("Id", (arg as Evento).id);
                descripcion = new XElement("Descripcion", (arg as Evento).descripcion);
                fechaInicio = new XElement("FechaInicio", (arg as Evento).fechaInicio);
                fechaFin = new XElement("FechaFin", (arg as Evento).fechaFin);
                inicio = (arg as Evento).fechaInicio;
                fin = (arg as Evento).fechaFin;
                localizacion = new XElement("Localizacion");
                latitud = new XElement("Latitud", (arg as Evento).localizacion.latitud);
                longitud = new XElement("Longitud", (arg as Evento).localizacion.longitud);
                localizacion.Add(latitud);
                localizacion.Add(longitud);

            }
            if (arg is Obra)
            {
                longitudS = ("" + (arg as Obra).localizacion.longitud).Replace(',', '.');
                latitudS = ("" + (arg as Obra).localizacion.latitud).Replace(',', '.');

                ambito = new XElement("Ambito", "Obra");
                id = new XElement("Id", (arg as Obra).id);
                descripcion = new XElement("Descripcion", (arg as Obra).descripcion);
                fechaInicio = new XElement("FechaInicio", (arg as Obra).fechaInicio);
                fechaFin = new XElement("FechaFin", (arg as Obra).fechaFin);
                inicio = (arg as Obra).fechaInicio;
                fin = (arg as Obra).fechaFin;
                localizacion = new XElement("Localizacion");
                latitud = new XElement("Latitud", (arg as Obra).localizacion.latitud);
                longitud = new XElement("Longitud", (arg as Obra).localizacion.longitud);
                localizacion.Add(latitud);
                localizacion.Add(longitud);
            }
            if (arg is Mantenimiento)
            {
                longitudS = ("" + (arg as Mantenimiento).localizacion.longitud).Replace(',', '.');
                latitudS = ("" + (arg as Mantenimiento).localizacion.latitud).Replace(',', '.');

                ambito = new XElement("Ambito", "Mantenimiento");
                id = new XElement("Id", (arg as Mantenimiento).id);
                descripcion = new XElement("Descripcion", (arg as Mantenimiento).descripcion);
                fechaInicio = new XElement("FechaInicio", (arg as Mantenimiento).fechaInicio);
                fechaFin = new XElement("FechaFin", (arg as Mantenimiento).fechaFin);
                inicio = (arg as Mantenimiento).fechaInicio;
                fin = (arg as Mantenimiento).fechaFin;
                localizacion = new XElement("Localizacion");
                latitud = new XElement("Latitud", (arg as Mantenimiento).localizacion.latitud);
                longitud = new XElement("Longitud", (arg as Mantenimiento).localizacion.longitud);
                localizacion.Add(latitud);
                localizacion.Add(longitud);
            }
            if (arg is Incidencia)
            {
                longitudS = ("" + (arg as Incidencia).localizacion.longitud).Replace(',', '.');
                latitudS = ("" + (arg as Incidencia).localizacion.latitud).Replace(',', '.');

                ambito = new XElement("Ambito", "Incidencia");
                id = new XElement("Id", (arg as Incidencia).id);
                descripcion = new XElement("Descripcion", (arg as Incidencia).descripcion);
                fechaInicio = new XElement("FechaInicio", (arg as Incidencia).fechaInicio);
                fechaFin = new XElement("FechaFin", (arg as Incidencia).fechaFin);
                inicio = (arg as Incidencia).fechaInicio;
                fin = (arg as Incidencia).fechaFin;
                localizacion = new XElement("Localizacion");
                latitud = new XElement("Latitud", (arg as Incidencia).localizacion.latitud);
                longitud = new XElement("Longitud", (arg as Incidencia).localizacion.longitud);
                localizacion.Add(latitud);
                localizacion.Add(longitud);
            }

            string url = "http://geocode.arcgis.com/arcgis/rest/services/World/GeocodeServer/reverseGeocode?location=" + longitudS + "%2C" + latitudS + "&distance=200&outSR=&f=pjson";
            string json = "";
            using (WebClient wc = new WebClient())
            {
                json = wc.DownloadString(url);
            }

            JToken obj = JObject.Parse(json);
            JToken direccion = obj.SelectToken("address");
            cp = int.Parse(direccion.SelectToken("Postal").Value<string>());
            cpIncidencia = cp;

            XElement codigoPostal = new XElement("CP", cp);

            XElement cuerpo = new XElement("Cuerpo");

            influencia.Add(codigoPostal);
            influencia.Add(barrio);
            influencia.Add(distrito);

            cabecera.Add(tipo);
            cabecera.Add(zona);
            cabecera.Add(influencia);
            cabecera.Add(fecha);

            incidencia.Add(cabecera);

            cuerpo.Add(id);
            cuerpo.Add(descripcion);
            cuerpo.Add(fechaInicio);
            cuerpo.Add(fechaFin);
            cuerpo.Add(localizacion);
            cuerpo.Add(ambito);

            incidencia.Add(cuerpo);

            Console.WriteLine(incidencia.ToString());
            return incidencia;
        }

        public XElement convertirTiempoCiudad(Dictionary<string, TiempoDiaCiudad> dt, string ciudad, Dictionary<string, string> es, Dictionary<string, string> eu)
        {
            XElement meteo = new XElement("TiempoCiudad");
            //Cabecera

            XElement cabecera = new XElement("Cabecera");
            XElement tipo = new XElement("Tipo", "tiempoCiudad");
            XElement zona = new XElement("Zona", "ZonaX");
            XElement fecha = new XElement("FechaDescarga", DateTime.Now);

            XElement influencia = new XElement("Influencia");
            XElement codigoPostal = new XElement("CP", 48000);
            XElement barrio = new XElement("Barrio", "Desconocido");
            XElement distrito = new XElement("Distrito", "Desconocido");
            influencia.Add(codigoPostal);
            influencia.Add(barrio);
            influencia.Add(distrito);

            cabecera.Add(tipo);
            cabecera.Add(zona);
            cabecera.Add(influencia);
            cabecera.Add(fecha);

            meteo.Add(cabecera);

            //Contenido

            XElement cuerpo = new XElement("Cuerpo");
            XElement xmlCiudad = new XElement("Ciudad");

            XElement nombre = new XElement("Nombre", ciudad);
            

            XElement hoy = new XElement("Hoy");
            XElement descripcionGeneral = new XElement("DescripcionGeneral");
            XElement descES = new XElement("ES", es["Hoy"]);
            XElement descEU = new XElement("EU", eu["Hoy"]);
            descripcionGeneral.Add(descES);
            descripcionGeneral.Add(descEU);

            XElement ciudadES = new XElement("DescripcionES", dt["Hoy"].descripcionES);
            XElement ciudadEU = new XElement("DescripcionEU", dt["Hoy"].descripcionEU);
            XElement max = new XElement("TempMax", dt["Hoy"].maxima);
            XElement min = new XElement("TempMin", dt["Hoy"].minima);

            hoy.Add(descripcionGeneral);
            hoy.Add(ciudadES);
            hoy.Add(ciudadEU);
            hoy.Add(max);
            hoy.Add(min);

            XElement manana = new XElement("Mañana");
            descripcionGeneral = new XElement("DescripcionGeneral");
            descES = new XElement("ES", es["Mañana"]);
            descEU = new XElement("EU", eu["Mañana"]);
            descripcionGeneral.Add(descES);
            descripcionGeneral.Add(descEU);

            ciudadES = new XElement("DescripcionES", dt["Mañana"].descripcionES);
            ciudadEU = new XElement("DescripcionEU", dt["Mañana"].descripcionEU);
            max = new XElement("TempMax", dt["Mañana"].maxima);
            min = new XElement("TempMin", dt["Mañana"].minima);

            manana.Add(descripcionGeneral);
            manana.Add(ciudadES);
            manana.Add(ciudadEU);
            manana.Add(max);
            manana.Add(min);

            XElement pasado = new XElement("Pasado");
            descripcionGeneral = new XElement("DescripcionGeneral");
            descES = new XElement("ES", es["Pasado"]);
            descEU = new XElement("EU", eu["Pasado"]);
            descripcionGeneral.Add(descES);
            descripcionGeneral.Add(descEU);

            ciudadES = new XElement("DescripcionES", dt["Pasado"].descripcionES);
            ciudadEU = new XElement("DescripcionEU", dt["Pasado"].descripcionEU);
            max = new XElement("TempMax", dt["Pasado"].maxima);
            min = new XElement("TempMin", dt["Pasado"].minima);

            pasado.Add(descripcionGeneral);
            pasado.Add(ciudadES);
            pasado.Add(ciudadEU);
            pasado.Add(max);
            pasado.Add(min);

            xmlCiudad.Add(nombre);
            XElement predicciones = new XElement("Predicciones");
            predicciones.Add(hoy);
            predicciones.Add(manana);
            predicciones.Add(pasado);
            xmlCiudad.Add(predicciones);

            cuerpo.Add(xmlCiudad);

            meteo.Add(cuerpo);

            Console.WriteLine(meteo.ToString());
            return meteo;
        }

        public string[] obtenerInfoPosicion(double latitud, double longitud)
        {
            string[] temporal = new string[3];

            return temporal;
        }
    }
}
