using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Clases;
using WrappersActiveMQ;

namespace ReceptorBus
{
    class Receptor
    {

        private IConnection _connection;
        private ISession _session;
        private const String QUEUE_DESTINATION = "PruebaEMISOR";
        private IMessageConsumer _consumer;


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
                _consumer = _session.CreateConsumer(destination);
                //producer.setDeliveryMode(DeliveryMode.NON_PERSISTENT);
                _consumer.Listener += _consumer_Listener;

            }
            catch (Exception e)
            {
                // TODO Auto-generated catch block
                Console.WriteLine(e.Message);
            }

        }

        private void _consumer_Listener(IMessage message)
        {
            ITextMessage temp = message as ITextMessage;
            //Console.WriteLine(temp.Text);

            switch (message.NMSType)
            {
                case "Clases.ParesParking":
                    var parkings = message.ToObject<ParesParking>();
                    if (parkings != null)
                    {
                        foreach (Clases.KeyValuePair<int, Parking> temp2 in parkings.lista)
                        {
                            Console.WriteLine(temp2.Value.nombre);
                        }
                    }
                    Console.ReadKey();
                    break;
                case "Clases.ParesTiempoCiudad":
                    var tiempoCiudad = message.ToObject<ParesTiempoCiudad>();
                    if (tiempoCiudad != null)
                    {
                        foreach (Clases.KeyValuePair<string, TiempoCiudad> temp2 in tiempoCiudad.lista)
                        {
                            Console.WriteLine(temp2.Value.descripcionES);
                        }
                    }
                    Console.ReadKey();
                    break;
                case "Clases.ParesTiempoComarca":
                    var tiempoComarca = message.ToObject<ParesTiempoComarca>();
                    if (tiempoComarca != null)
                    {
                        foreach (Clases.KeyValuePair<string, TiempoComarca> temp2 in tiempoComarca.lista)
                        {
                            Console.WriteLine(temp2.Value.nombreComarcaEs);
                        }
                    }
                    break;
                case "Clases.TiempoPrediccion":
                    var tiempoPrediccion = message.ToObject<Clases.TiempoPrediccion>();
                    if (tiempoPrediccion != null)
                    {
                        foreach (TiempoDia temporal in tiempoPrediccion.predicciones)
                        {
                            Console.WriteLine(temporal.temperaturaES+"\r\n");
                        }
                    }
                    break;
                case "WrappersActiveMQ.WrapperEvento":
                    var eventos = message.ToObject<WrapperEvento>();
                    if (eventos != null)
                    {
                        if (eventos.lista.Count != 0)
                        {
                            foreach (Evento temporal in eventos.lista)
                            {
                                Console.WriteLine(temporal.descripcion + "\r\n");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No hay nada que mostrar");
                        }
                        
                    }

                    break;
                case "WrappersActiveMQ.WrapperObra":
                    var obras = message.ToObject<WrapperObra>();
                    if (obras != null)
                    {
                        if (obras.lista.Count != 0)
                        {
                            foreach (Obra temporal in obras.lista)
                            {
                                Console.WriteLine(temporal.descripcion + "\r\n");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No hay nada que mostrar");
                        }

                    }

                    break;
                case "WrappersActiveMQ.WrapperIncidencia":
                    var incidencias = message.ToObject<WrapperIncidencia>();
                    if (incidencias != null)
                    {
                        if (incidencias.lista.Count != 0)
                        {
                            foreach (Incidencia temporal in incidencias.lista)
                            {
                                Console.WriteLine(temporal.descripcion + "\r\n");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No hay nada que mostrar");
                        }

                    }
                    break;
                case "WrappersActiveMQ.WrapperMantenimiento":
                    var mantenimiento = message.ToObject<WrapperMantenimiento>();
                    if (mantenimiento != null)
                    {
                        if (mantenimiento.lista.Count != 0)
                        {
                            foreach (Mantenimiento temporal in mantenimiento.lista)
                            {
                                Console.WriteLine(temporal.descripcion + "\r\n");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No hay nada que mostrar");
                        }

                    }
                    break;
                case "WrappersActiveMQ.ParesBilbobus":
                    var bilbobus = message.ToObject<ParesBilbobus>();
                    if (bilbobus != null)
                    {
                        if (bilbobus.lista.Count != 0)
                        {
                            foreach (Clases.KeyValuePair<int, ParadaBilbo> temporal in bilbobus.lista)
                            {
                                foreach (Clases.KeyValuePair<string, LineaBusTiempo> temporal2 in temporal.Value.lineasYTiempo)
                                {
                                    Console.WriteLine(temporal2.Value.descripcionLinea + "\r\n");
                                }
                                
                            }
                        }
                        else
                        {
                            Console.WriteLine("No hay nada que mostrar");
                        }

                    }
                    break;
                case "WrappersActiveMQ.ParesEuskotren":
                    var euskotren = message.ToObject<ParesEuskotren>();
                    if (euskotren != null)
                    {
                        if (euskotren.lista.Count != 0)
                        {
                            foreach (Clases.KeyValuePair<int, ParadaEuskotren> temporal in euskotren.lista)
                            {
                                  Console.WriteLine(temporal.Value.nombre + "\r\n");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No hay nada que mostrar");
                        }

                    }
                    break;
                case "WrappersActiveMQ.ParesBizkaibus":
                    var bizkaibus = message.ToObject<ParesBizkaibus>();
                    if (bizkaibus != null)
                    {
                        if (bizkaibus.lista.Count != 0)
                        {
                            foreach (Clases.KeyValuePair<int, ParadaBizkaibus> temporal in bizkaibus.lista)
                            {
                                Console.WriteLine(temporal.Value.nombreParada + "\r\n");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No hay nada que mostrar");
                        }

                    }
                    break;
                case "WrappersActiveMQ.WrapperFarmacia":
                    var farmacias = message.ToObject<WrapperFarmacia>();
                    if (farmacias != null)
                    {
                        if (farmacias.lista.Count != 0)
                        {
                            foreach (Farmacia temporal in farmacias.lista)
                            {
                                Console.WriteLine(temporal.nombreFarmacia + "\r\n");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No hay nada que mostrar");
                        }

                    }
                    break;
                case "WrappersActiveMQ.WrapperHospital":
                    var hospitales = message.ToObject<WrapperHospital>();
                    if (hospitales != null)
                    {
                        if (hospitales.lista.Count != 0)
                        {
                            foreach (Hospital temporal in hospitales.lista)
                            {
                                Console.WriteLine(temporal.nombreHospital + "\r\n");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No hay nada que mostrar");
                        }

                    }
                    break;
                case "WrappersActiveMQ.WrapperCentroSalud":
                    var centros = message.ToObject<WrapperCentroSalud>();
                    if (centros != null)
                    {
                        if (centros.lista.Count != 0)
                        {
                            foreach (CentroSalud temporal in centros.lista)
                            {
                                Console.WriteLine(temporal.nombreCentro + "\r\n");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No hay nada que mostrar");
                        }

                    }
                    break;
                case "WrappersActiveMQ.WrapperBicicleta":
                    var bicis = message.ToObject<WrapperBicicleta>();
                    if (bicis != null)
                    {
                        if (bicis.lista.Count != 0)
                        {
                            foreach (PuntoBici temporal in bicis.lista)
                            {
                                Console.WriteLine(temporal.nombre + "\r\n");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No hay nada que mostrar");
                        }

                    }
                    break;
                case "WrappersActiveMQ.WrapperParkingDeusto":
                    var deusto = message.ToObject<WrapperParkingDeusto>();
                    if (deusto != null)
                    {
                        Console.WriteLine("Parking Deusto: +DBS: "+deusto.contadorDBS+" +General: "+deusto.contadorGeneral);
                    }
                    break;
                case "WrappersActiveMQ.ParesLineasBilbobus":
                    var lineasBilbobus = message.ToObject<ParesLineasBilbobus>();
                    if (lineasBilbobus != null)
                    {
                        if (lineasBilbobus.lista.Count != 0)
                        {
                            /*foreach (Clases.KeyValuePair<string, LineaBilbobus> temporal in lineasBilbobus.lista)
                            {
                                foreach (Clases.KeyValuePair<int, ViajeBilbobus> temporal2 in temporal.Value.viajes)
                                {
                                    Console.WriteLine(temporal2.Value.id);
                                    foreach (int temporal3 in temporal2.Value.clavesParada)
                                    {
                                        Console.WriteLine(temporal3);
                                    }
                                }

                            }*/
                            Console.WriteLine("He terminado");
                        }
                        else
                        {
                            Console.WriteLine("No hay nada que mostrar");
                        }

                    }
                    break;
                case "WrappersActiveMQ.ParesLineasEuskotren":
                    var lineasEuskotren = message.ToObject<ParesLineasEuskotren>();
                    if (lineasEuskotren != null)
                    {
                        if (lineasEuskotren.lista.Count != 0)
                        {
                            /*foreach (Clases.KeyValuePair<int, LineaEuskotren> temporal in lineasEuskotren.lista)
                            {
                                foreach (Clases.KeyValuePair<string, ViajeEuskotren> temporal2 in temporal.Value.viajes)
                                {
                                    Console.WriteLine(temporal2.Value.id);
                                    foreach (int temporal3 in temporal2.Value.clavesParada)
                                    {
                                        Console.WriteLine(temporal3);
                                    }
                                }

                            }*/
                            Console.WriteLine("He terminado");
                        }
                        else
                        {
                            Console.WriteLine("No hay nada que mostrar");
                        }

                    }
                    break;
                case "WrappersActiveMQ.ParesLineasBizkaibus":
                    var lineasBizkaibus = message.ToObject<ParesLineasBizkaibus>();
                    if (lineasBizkaibus != null)
                    {
                        if (lineasBizkaibus.lista.Count != 0)
                        {
                            /*foreach (Clases.KeyValuePair<int, LineaBizkaibus> temporal in lineasBizkaibus.lista)
                            {
                                foreach (Clases.KeyValuePair<int, ViajeBizkaibus> temporal2 in temporal.Value.viajes)
                                {
                                    //Console.WriteLine(temporal2.Value.id);
                                    foreach (int temporal3 in temporal2.Value.clavesParada)
                                    {
                                        Console.WriteLine(temporal3);
                                    }
                                }

                            }*/
                            Console.WriteLine("He terminado");
                        }
                        else
                        {
                            Console.WriteLine("No hay nada que mostrar");
                        }

                    }
                    break;
                case "WrappersActiveMQ.ParesLineaMetro":
                    var lineasMetro = message.ToObject<ParesLineaMetro>();
                    if (lineasMetro != null)
                    {
                        if (lineasMetro.lista.Count != 0)
                        {
                            /*foreach (Clases.KeyValuePair<string, LineaMetro> temporal in lineasMetro.lista)
                            {
                                foreach (Clases.KeyValuePair<int, ViajeMetro> temporal2 in temporal.Value.viajes)
                                {
                                    //Console.WriteLine(temporal2.Value.id);
                                    foreach (Clases.KeyValuePair<int, ParadaMetro>temporal3 in temporal2.Value.paradasList)
                                    {
                                        Console.WriteLine(temporal3.Value.nombreParada);
                                    }
                                }

                            }*/
                            Console.WriteLine("He terminado");
                        }
                        else
                        {
                            Console.WriteLine("No hay nada que mostrar");
                        }

                    }
                    break;


            }

        }

        static void Main(string[] args)
        {
            Console.WriteLine("Receptor 1");
            Receptor r = new Receptor();
            r.inicializar();
            while (true)
            {

            }
        }

    }
}
