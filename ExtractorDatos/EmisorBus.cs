using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.ActiveMQ.Commands;
using Clases;
using WrappersActiveMQ;

namespace ExtractorDatos
{
    internal class EmisorBus
    {
        private IConnection _connection;
        private ISession _session;
        private const String QUEUE_DESTINATION = "PruebaEMISOR";
        private IMessageProducer _producer;

        public void inicializar()
        {
            // configure the broker
            try
            {
                // Create a ConnectionFactory
                IConnectionFactory connectionFactory = new ConnectionFactory(
                    "tcp://localhost:61616");

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

        public void enviarParkings(Dictionary<int, Parking> m)
        {
            //IObjectMessage objecto = _producer.CreateObjectMessage(m);
            try
            {
                List<System.Collections.Generic.KeyValuePair<int, Parking>> temporal = m.ToList();
                List<Clases.KeyValuePair<int, Parking>> miKeyValue = new List<Clases.KeyValuePair<int, Parking>>();
                foreach (System.Collections.Generic.KeyValuePair<int, Parking> valuePair in temporal)
                {
                    miKeyValue.Add(new Clases.KeyValuePair<int, Parking>(valuePair.Key, valuePair.Value));
                }
                _producer.Send(new ParesParking(miKeyValue));
            }
            catch (System.NullReferenceException ex)
            {
                Console.WriteLine("No se ha podido inicializar el productor. Compruebe que ApacheMQ esté encendido.");
                Console.ReadKey();
            }

        }

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
                    Console.WriteLine(pair.Value.nombreComarcaEs);
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

        public void enviarParadasBilbobus(Dictionary<int, ParadaBilbo> m)
        {
            //IObjectMessage objecto = _producer.CreateObjectMessage(m);
            try
            {
                List<System.Collections.Generic.KeyValuePair<int, ParadaBilbo>> temporal = m.ToList();
                List<Clases.KeyValuePair<int, ParadaBilbo>> miKeyValue = new List<Clases.KeyValuePair<int, ParadaBilbo>>();
                foreach (System.Collections.Generic.KeyValuePair<int, ParadaBilbo> valuePair in temporal)
                {
                    miKeyValue.Add(new Clases.KeyValuePair<int, ParadaBilbo>(valuePair.Key, valuePair.Value));
                }
                _producer.Send(new ParesBilbobus(miKeyValue));
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

        public void enviarBicicletas(List<PuntoBici> bicis)
        {
            try
            {
                _producer.Send(new WrapperBicicleta(bicis));
            }
            catch (System.NullReferenceException ex)
            {
                Console.WriteLine("No se ha podido inicializar el productor. Compruebe que ApacheMQ esté encendido.");
                Console.ReadKey();
            }
        }

        public void enviarParkingDeusto(int dbs, int general)
        {
            try
            {
                _producer.Send(new WrapperParkingDeusto(dbs, general));
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
    }
}
