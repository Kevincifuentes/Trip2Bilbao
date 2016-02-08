using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Data.SqlTypes;
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
            //Obtengo estático
            contexto = new ModeloContainer();
            //ObtenerEstatico oe = new ObtenerEstatico(contexto);
            //obtenerBicis(oe);

            //obtenerParkings(oe);
            //obtenerCentros(oe);
            //obtenerHospitales(oe);
            //obtenerFarmacias(oe);
            
            //obtenerTranvia(oe);
            //obtenerBilbo(oe);
            //obtenerEuskotren(oe);
            //obtenerMetro(oe);
            //obtenerBizkaibus(oe);
            //obtenerBilbobus(oe);

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

        public void obtenerBilbobus(ObtenerEstatico oe)
        {
            oe.obtenerBilbao();
        }

        public void obtenerEuskotren(ObtenerEstatico oe)
        {
            oe.obtenerEuskotren();
        }

        public void obtenerMetro(ObtenerEstatico oe)
        {
            oe.obtenerMetro();
        }

        public void obtenerBizkaibus(ObtenerEstatico oe)
        {
            oe.obtenerBizkaibus();
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
                    if (bici != null && !message.NMSRedelivered)
                    {
                        almacenarBicis(bici);
                    }
                    break;
                case "Deusto":
                    XElement deustoP = XElement.Parse(temp.Text);
                    XmlDocument deusto = new XmlDocument();
                    deusto.LoadXml(temp.Text);
                    if (deustoP != null && !message.NMSRedelivered)
                    {
                        almacenarDeusto(deusto);
                        
                    }
                    break;
                case "Incidencia":
                    //XElement incidencia = XElement.Parse(temp.Text);
                    XmlDocument incidencia = new XmlDocument();
                    incidencia.LoadXml(temp.Text);
                    if (incidencia != null && !message.NMSRedelivered)
                    {
                        //Console.WriteLine(bici.InnerXml);
                        almacenarIncidencia(incidencia);
                    }
                    break;
                case "TiempoCiudad":
                    XmlDocument tiempo = new XmlDocument();
                    tiempo.LoadXml(temp.Text);
                    if (tiempo != null && !message.NMSRedelivered)
                    {
                        Console.WriteLine(tiempo.ToString());
                        almacenarTiempo(tiempo);
                    }
                    break;
            }

        }

        private void almacenarTiempo(XmlDocument tiempo)
        {
            tiempos_dia_ciudad tdc = new tiempos_dia_ciudad();
            tdc.fechaDescarga = DateTime.Parse(tiempo.ChildNodes[1].ChildNodes[0].ChildNodes[3].InnerText);
            tdc.nombreCiudad = tiempo.ChildNodes[1].ChildNodes[1].ChildNodes[0].ChildNodes[0].InnerText;

            foreach (XmlNode node in tiempo.ChildNodes[1].ChildNodes[1].ChildNodes[0].ChildNodes[1].ChildNodes)
            {
                tiempos_ciudad tc1 = new tiempos_ciudad();
                tc1.diaPrediccion = node.Name;
                tc1.descripcionGeneralES =
                    node.ChildNodes[0].ChildNodes[0].InnerText;
                tc1.descripcionGeneralEU =
                    node.ChildNodes[0].ChildNodes[1].InnerText;
                tc1.descripcionES = node.ChildNodes[1].InnerText;
                tc1.descripcionEU = node.ChildNodes[2].InnerText;
                tc1.maxima = int.Parse(node.ChildNodes[3].InnerText);
                tc1.minima = int.Parse(node.ChildNodes[4].InnerText);
            
                tdc.tiempos_ciudad.Add(tc1);
            }
            contexto.tiempos_dia_ciudadSet.Add(tdc);
            try
            {
                contexto.SaveChanges();
                Console.WriteLine(">>>>>Insercción de Incidencias (T.R.) realizada<<<<<");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
        }

        private void almacenarIncidencia(XmlDocument incidencia)
        {
            incidencias i = new incidencias();
            i.id = int.Parse(incidencia.ChildNodes[1].ChildNodes[1].ChildNodes[0].InnerText);
            i.tipo = incidencia.ChildNodes[1].ChildNodes[1].ChildNodes[5].InnerText;
            i.descripcion = incidencia.ChildNodes[1].ChildNodes[1].ChildNodes[1].InnerText;
            i.fechaInicio = DateTime.Parse(incidencia.ChildNodes[1].ChildNodes[1].ChildNodes[2].InnerText);
            i.fechaFin = DateTime.Parse(incidencia.ChildNodes[1].ChildNodes[1].ChildNodes[3].InnerText);
            i.latitud = double.Parse(incidencia.ChildNodes[1].ChildNodes[1].ChildNodes[4].ChildNodes[0].InnerText);
            i.longitud = double.Parse(incidencia.ChildNodes[1].ChildNodes[1].ChildNodes[4].ChildNodes[1].InnerText);

            contexto.incidenciasSet.Add(i);
            try
            {
                contexto.SaveChanges();
                Console.WriteLine(">>>>>Insercción de Incidencias (T.R.) realizada<<<<<");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
        }

        private void almacenarBicis(XmlDocument bici)
        {
            estados_puntobici ep = new estados_puntobici();
            ep.puntos_bici_id = int.Parse(bici.ChildNodes[1].ChildNodes[1].ChildNodes[0].InnerText);
            ep.fecha = DateTime.Parse(bici.ChildNodes[1].ChildNodes[0].ChildNodes[3].InnerText);
            ep.anclajeslibres = int.Parse(bici.ChildNodes[1].ChildNodes[1].ChildNodes[5].InnerText);
            ep.bicisLibres = int.Parse(bici.ChildNodes[1].ChildNodes[1].ChildNodes[4].InnerText);
            contexto.estados_puntobiciSet.Add(ep);
            try
            {
                contexto.SaveChanges();
                Console.WriteLine(">>>>>Insercción de BICIS (T.R.) realizada<<<<<");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
        }

        private void almacenarDeusto(XmlDocument deusto)
        {
            int dbs, general;
            dbs = int.Parse(deusto.ChildNodes[1].ChildNodes[1].ChildNodes[0].InnerText);
            general = int.Parse(deusto.ChildNodes[1].ChildNodes[1].ChildNodes[1].InnerText);
            Console.WriteLine("Resultado: " + dbs + "/" + general);
            parkingDeusto p = new parkingDeusto();
            p.dbs = dbs;
            p.general = general;
            p.fecha = DateTime.Parse(deusto.ChildNodes[1].ChildNodes[0].ChildNodes[3].InnerText);
            contexto.parkingDeustoSet.Add(p);
            try
            {
                contexto.SaveChanges();
                Console.WriteLine(">>>>>Insercción de PARKING DEUSTO (T.R.) realizada<<<<<");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
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
