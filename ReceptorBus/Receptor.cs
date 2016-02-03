using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Timers;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Almacenamiento;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Clases;
using ExtractorDatos;
using WrappersActiveMQ;

namespace ReceptorBus
{
    class Receptor
    {

        private IConnection _connection;
        private ISession _session;
        private const String QUEUE_DESTINATION = "PruebaEMISOR";
        private IMessageConsumer _consumer;
        private ModeloContainer contexto;


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


            //Obtengo estático
            contexto = new ModeloContainer();
            ObtenerEstatico oe = new ObtenerEstatico(contexto);
            //obtenerParkings(oe);
            //obtenerCentros(oe);
            //obtenerHospitales(oe);
            //obtenerFarmacias(oe);
            //obtenerBicis(oe);
            //obtenerTranvia(oe);
            //obtenerBilbo(oe);
            obtenerEuskotren(oe);

        }

        public void obtenerParkings(ObtenerEstatico oe)
        {
                oe.parkingEstatico();
            
        }

        public void obtenerCentros(ObtenerEstatico oe)
        {
                oe.obtenerCentrosSalud();

        }

        public void obtenerHospitales(ObtenerEstatico oe)
        {
            oe.obtenerHospitales();

        }

        public void obtenerFarmacias(ObtenerEstatico oe)
        {
            oe.obtenerFarmacias();

        }

        public void obtenerBicis(ObtenerEstatico oe)
        {
            oe.obtenerBicis();
        }

        public void obtenerTranvia(ObtenerEstatico oe)
        {
            oe.obtenerTranvia();
        }

        public void obtenerBilbo(ObtenerEstatico oe)
        {
            oe.obtenerBilbao();
        }

        public void obtenerEuskotren(ObtenerEstatico oe)
        {
            oe.obtenerEuskotren();
        }

        private void _consumer_Listener(IMessage message)
        {
            ITextMessage temp = message as ITextMessage;
            //Console.WriteLine(temp.Text);

            switch (message.NMSType)
            {
                case "Parkings":
                   // XElement parking = XElement.Parse(temp.Text);
                     XmlDocument parking = new XmlDocument();
                    parking.LoadXml(temp.Text);
                    if (parking != null)
                    {
                        Console.WriteLine(parking.InnerXml);
                        Console.WriteLine(parking.ChildNodes[1].Name);
                    }
                    break;
                case "TiemposParada":
                    XElement parada = XElement.Parse(temp.Text);
                    //XmlDocument parada = new XmlDocument();
                    //parada.LoadXml(temp.Text);
                    if (parada != null)
                    {
                        Console.WriteLine(parada.ToString());
                    }
                    break;
                case "TiemposLinea":
                    XElement linea = XElement.Parse(temp.Text);
                    //XmlDocument linea = new XmlDocument();
                    //linea.LoadXml(temp.Text);
                    if (linea != null)
                    {
                        Console.WriteLine(linea.ToString());
                    }
                    break;
                case "Bicis":
                    //XElement bici = XElement.Parse(temp.Text);
                    XmlDocument bici = new XmlDocument();
                    bici.LoadXml(temp.Text);
                    if (bici != null)
                    {
                        //Console.WriteLine(bici.InnerXml);
                        Console.WriteLine(bici.InnerXml);
                    }
                    break;
                case "Deusto":
                    XElement deustoP = XElement.Parse(temp.Text);
                    //XmlDocument deusto = new XmlDocument();
                    //bici.LoadXml(temp.Text);
                    if (deustoP != null)
                    {
                        //Console.WriteLine(bici.InnerXml);
                        parkingDeusto p = new parkingDeusto();
                        Console.WriteLine(deustoP.ToString());
                    }
                    break;
                case "Incidencia":
                    XElement incidencia = XElement.Parse(temp.Text);
                    //XmlDocument deusto = new XmlDocument();
                    //bici.LoadXml(temp.Text);
                    if (incidencia != null)
                    {
                        //Console.WriteLine(bici.InnerXml);
                        Console.WriteLine(incidencia.ToString());
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
