using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Almacenamiento;
using Apache.NMS.ActiveMQ.Transport;
using Clases;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using WrappersActiveMQ;

namespace ExtractorDatos
{
    class ObtenerEstatico
    {
        

        public ObtenerEstatico(ModeloContainer context)
        {
            contexto = context;
        }

        public ModeloContainer contexto;
        public Dictionary<int, parkings> parkingslist = new Dictionary<int, parkings>();
        public Dictionary<int, paradas_bilbobus> paradasBilbobus = new Dictionary<int, paradas_bilbobus>();
        private List<paradas_tranvia> paradasTranvia = new List<paradas_tranvia>(); 
        private Dictionary<int, paradas_bizkaibus> paradasBizkaibus= new Dictionary<int, paradas_bizkaibus>();
        private Dictionary<string , paradas_metro> paradasMetro = new Dictionary<string, paradas_metro>(); 
        private Dictionary<int, paradas_euskotren> paradasEuskotren = new Dictionary<int, paradas_euskotren>();
        public Dictionary<string, lineas_bilbobus> lineasBilbo = new Dictionary<string, lineas_bilbobus>();
        private Dictionary<int, lineas_euskotren> lineasEusko = new Dictionary<int, lineas_euskotren>();
        private Dictionary<int, lineas_bizkaibus> lineasBizkaia = new Dictionary<int, lineas_bizkaibus>();
        private Dictionary<string, lineas_metro> lineasMetroBilbao = new Dictionary<string, lineas_metro>();  
        private List<farmacias> farmaciasList = new List<farmacias>(); 
        private List<hospitales> hospitalList = new List<hospitales>();
        private List<centros_de_salud> centroSaludList = new List<centros_de_salud>();
        private List<puntos_bici> puntosBicisList = new List<puntos_bici>(); 
        private FileStream fichero;
        

        private XmlDocument descargaDeURL(string url)
        {
            if (fichero != null)
            {
                fichero.Dispose();
            }

            XmlDocument document = new XmlDocument();
            try
            {
                document.Load(url);
               /* StreamReader reader = new StreamReader(WebRequest.Create(url).GetResponse().GetResponseStream(), Encoding.GetEncoding("UTF-8"));
                document.LoadXml(reader.ReadToEnd());*/

            }
            catch(System.Net.WebException e)
            {
                Console.WriteLine("Error al intentar obtener la url (WebException): " + url);
                WebClient webClient = new WebClient();
                webClient.DownloadFile(url, @"C:\Users\Kevin\Documents\Visual Studio 2013\Projects\ExtractorDatos\ReceptorBus\temporal\temporal.xml");
                File.WriteAllText(@"C:\Users\Kevin\Documents\Visual Studio 2013\Projects\ExtractorDatos\ReceptorBus\temporal\temporal.xml", Regex.Replace(File.ReadAllText(@"C:\Users\Kevin\Documents\Visual Studio 2013\Projects\ExtractorDatos\ReceptorBus\temporal\temporal.xml"), @"<!DOCTYPE sanidad_ubicacion SYSTEM ""/iwmnt/euskadiplus/main/r01_system/WORKAREA/wr01_system/templatedata/sanidad/sanidad_ubicacion/sanidad_ubicacion.dtd"">", ""));
                fichero =
                    File.Open(
                        @"C:\Users\Kevin\Documents\Visual Studio 2013\Projects\ExtractorDatos\ReceptorBus\temporal\temporal.xml",
                        FileMode.Open);
                try
                {
                    document.Load(new StreamReader(fichero, Encoding.GetEncoding("UTF-8")));
                }
                catch (XmlException ex)
                {
                    document = null;
                }
                //document = null;
            }
            catch (System.Xml.XmlException ex)
            {
                Console.WriteLine("Error al intentar obtener la url (XmlException): " + url);
                Console.Write(ex.Message);
                WebClient webClient = new WebClient();
                webClient.DownloadFile(url, @"C:\Users\Kevin\Documents\Visual Studio 2013\Projects\ExtractorDatos\ReceptorBus\temporal\temporal.xml");
                File.WriteAllText(@"C:\Users\Kevin\Documents\Visual Studio 2013\Projects\ExtractorDatos\ReceptorBus\temporal\temporal.xml", Regex.Replace(File.ReadAllText(@"C:\Users\Kevin\Documents\Visual Studio 2013\Projects\ExtractorDatos\ReceptorBus\temporal\temporal.xml"), @"<!DOCTYPE sanidad_ubicacion SYSTEM ""/iwmnt/euskadiplus/main/r01_system/WORKAREA/wr01_system/templatedata/sanidad/sanidad_ubicacion/sanidad_ubicacion.dtd"">", ""));
                fichero =
                    File.Open(
                        @"C:\Users\Kevin\Documents\Visual Studio 2013\Projects\ExtractorDatos\ReceptorBus\temporal\temporal.xml",
                        FileMode.Open);
                try
                {
                    document.Load(new StreamReader(fichero, Encoding.GetEncoding("UTF-8")));
                }
                catch (XmlException ex2)
                {
                    document = null;
                }
                
                //document = null;
                
            }
            return document;
        }

        //Obtengo la información estática mediante lo que he obtenido dinámicamente

        public void parkingEstatico()
        {
            contexto.parkingsSet.RemoveRange(contexto.parkingsSet);
            contexto.SaveChanges();
            recintosAparcamiento();
            recintosAparcamientoEstatico();
            contexto.parkingsSet.AddRange(parkingslist.Values.Where(p => p.entradas.Count != 0));
            try
            {
                contexto.SaveChanges();
                Console.WriteLine(">>>>>Insercción de PARKING realizada<<<<<");
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        public void obtenerCentrosSalud()
        {
            contexto.centros_de_saludSet.RemoveRange(contexto.centros_de_saludSet);
            contexto.SaveChanges();
            centrosDeSalud();
            contexto.centros_de_saludSet.AddRange(centroSaludList);
            try
            {
                contexto.SaveChanges();
                
                Console.WriteLine(">>>>>Insercción de CENTROS DE SALUD realizada<<<<<");
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        public void obtenerHospitales()
        {
            contexto.hospitalesSet.RemoveRange(contexto.hospitalesSet);
            contexto.SaveChanges();
            hospitales();
            contexto.hospitalesSet.AddRange(hospitalList);
            try
            {
                contexto.SaveChanges();
                Console.WriteLine(">>>>>Insercción de HOSPITALES realizada<<<<<");
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        public void obtenerFarmacias()
        {
            contexto.farmaciasSet.RemoveRange(contexto.farmaciasSet);
            contexto.SaveChanges();
            farmacias();
            contexto.farmaciasSet.AddRange(farmaciasList);
            try
            {
                contexto.SaveChanges();
                Console.WriteLine(">>>>>Insercción de FARMACIAS realizada<<<<<");
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        public void obtenerBicis()
        {
            contexto.puntos_biciSet.RemoveRange(contexto.puntos_biciSet);
            contexto.SaveChanges();
            bicicletas();
            contexto.puntos_biciSet.AddRange(puntosBicisList);
            try
            {
                contexto.SaveChanges();
                Console.WriteLine(">>>>>Insercción de BICIS realizada<<<<<");
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        public void obtenerEuskotren()
        {
            contexto.lineas_euskotrenSet.RemoveRange(contexto.lineas_euskotrenSet);
            contexto.paradas_euskotrenSet.RemoveRange(contexto.paradas_euskotrenSet);
            contexto.SaveChanges();
            euskotren();
            lineasEuskotren();
            contexto.lineas_euskotrenSet.AddRange(lineasEusko.Values);
            try
            {
                contexto.SaveChanges();
                Console.WriteLine(">>>>>Insercción de EUSKOTREN realizada<<<<<");
            }
            catch (DbEntityValidationException e)
            {
                
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        public void obtenerBilbao()
        {
            contexto.lineas_bilbobusSet.RemoveRange(contexto.lineas_bilbobusSet);
            contexto.paradas_bilbobusSet.RemoveRange(contexto.paradas_bilbobusSet);
            contexto.SaveChanges();
            paradasAutobusesBilbo();
            lineasBilbobus();
            contexto.lineas_bilbobusSet.AddRange(lineasBilbo.Values);
            try
            {
                contexto.SaveChanges();
                Console.WriteLine(">>>>>Insercción de BILBOBUS realizada<<<<<");
            }
            catch (DbEntityValidationException e)
            {

                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        public void obtenerBizkaibus()
        {
            contexto.lineas_bizkaibusSet.RemoveRange(contexto.lineas_bizkaibusSet);
            contexto.paradas_bizkaibusSet.RemoveRange(contexto.paradas_bizkaibusSet);
            contexto.SaveChanges();
            paradasAutobusesBizkaia();
            lineasBizkaibus();
            contexto.lineas_bizkaibusSet.AddRange(lineasBizkaia.Values);
            try
            {
                contexto.SaveChanges();
                Console.WriteLine(">>>>>Insercción de BIZKAIBUS realizada<<<<<");
            }
            catch (DbEntityValidationException e)
            {

                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        public void obtenerTranvia()
        {
            contexto.paradas_tranviaSet.RemoveRange(contexto.paradas_tranviaSet);
            contexto.SaveChanges();
            tranvia();
            contexto.paradas_tranviaSet.AddRange(paradasTranvia);
            try
            {
                contexto.SaveChanges();
                Console.WriteLine(">>>>>Insercción de TRANVIA realizada<<<<<");
            }
            catch (DbEntityValidationException e)
            {

                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        public void obtenerMetro()
        {
            contexto.lineas_metroSet.RemoveRange(contexto.lineas_metroSet);
            contexto.paradas_metroSet.RemoveRange(contexto.paradas_metroSet);
            contexto.SaveChanges();
            metroBilbao();
            lineasMetro();
            contexto.lineas_metroSet.AddRange(lineasMetroBilbao.Values);
            try
            {
                contexto.SaveChanges();
                Console.WriteLine(">>>>>Insercción de METRO realizada<<<<<");
            }
            catch (DbEntityValidationException e)
            {

                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        private void recintosAparcamiento()
        {
            if (parkingslist.Count != 0)
            {
                parkingslist.Clear();
            }

            Console.WriteLine("Empiezo Aparcamiento");
            XmlDocument aparcamientosxml=this.descargaDeURL("http://www.geobilbao.net/wfsCocities?service=wfs&version=1.1.0&request=GetFeature&typeName=eti:CarParkDynamic");

            //Obtengo los nodos desde el padre de todos
            if (aparcamientosxml != null)
            {
                XmlNodeList temp = aparcamientosxml.SelectNodes("//*");

                Console.WriteLine("Empiezo Foreach");

                //Con esto obtengo los PARKINGS unicamente
                foreach (XmlNode node in temp[0].ChildNodes[0].ChildNodes)
                {
                    XmlAttributeCollection atributos = node.Attributes;
                    //Console.WriteLine("paso"+ node.Name);

                    //Obtiene el class del parking (id)
                    string tipo = atributos.Item(0).InnerText;
                    Console.WriteLine(tipo);

                    //Obtenemos unicamente el ID
                    string id = tipo.Substring(15, 6);
                    Console.WriteLine("ID: " + id);

                    /* Añadir a una clase*/
                    parkings temporal = new parkings();
                    temporal.id = int.Parse(id);
                    Console.WriteLine("Despues del parse: " + temporal.id);


                    //Empezamos a obtener los datos para ese ID especifico
                    // Nodo por Nodo
                    int indice = 0;
                    Console.WriteLine(node.ChildNodes[indice].FirstChild);

                    //Ocupacion
                    if (node.ChildNodes[indice].Name.Equals("eti:occupancy"))
                    {
                        indice++;
                    }
                    else
                    {
                        //No hay información
                    }

                    //Porcentaje
                    if (node.ChildNodes[indice].Name.Equals("eti:occupancyPercentage"))
                    {
                        indice++;
                    }
                    else
                    {
                        //No hay información
                    }

                    //Llegado a este punto, en todos los casos estaríamos en la etiqueta lastupdate
                    string fecha = node.ChildNodes[indice].InnerText;
                    int ano = int.Parse(fecha.Substring(0, 4));
                    int mes = int.Parse(fecha.Substring(5, 2));
                    int dia = int.Parse(fecha.Substring(8, 2));
                    int hora = int.Parse(fecha.Substring(11, 2));
                    int minutos = int.Parse(fecha.Substring(14, 2));
                    int segundos = int.Parse(fecha.Substring(17, 2));
                    DateTime ne = new DateTime(ano, mes, dia, hora, minutos, segundos);
                    temporal.fecha = ne;

                    //Aumentamos el indice
                    indice++;

                    //Ahora obtenemos el estado
                    temporal.estado = node.ChildNodes[indice].InnerText;

                    //Con esto hemos terminado la obtención de los datos dinámicos.
                    //Añadimos el Parking al Dictionary
                    parkingslist.Add(temporal.id, temporal);
                }
            }
            else
            {
                Console.WriteLine("No se ha podido obtener los recintos de aparcamiento. Compruebe su conexión a internet.");
            }
           
        }

        //Obtener información estática sobre los parkings con los ids ya almacenados
        private void recintosAparcamientoEstatico()
        {

            Console.WriteLine("Empiezo Aparcamiento Estatico");
            XmlDocument aparcamientosxml = this.descargaDeURL("http://www.geobilbao.net/wfsCocities?service=wfs&version=1.1.0&request=GetFeature&typeName=edi:ParkingPoint");

            /* XmlNamespaceManager nsm = new XmlNamespaceManager(aparcamientosxml.NameTable);
             nsm.AddNamespace("wfs", "http://www.geobilbao.net/wfsCocities/schemas/wfs/1.1.0/wfs.xsd");
             nsm.AddNamespace("gml", "http://schemas.opengis.net/gml/3.1.1/base/gml.xsd");*/
            if (aparcamientosxml != null)
            {
                XmlNodeList temp = aparcamientosxml.SelectNodes("//*");

                Console.WriteLine("Empiezo Foreach Estatico");

                //Con esto obtengo los PARKINGS unicamente
                foreach (XmlNode node in temp[0].ChildNodes[0].ChildNodes)
                {
                    //Variables a dar valor
                    string nombreParking = "Desconocido";
                    Coordenadas c = new Coordenadas();
                    List<entradas> entradas = new List<entradas>();
                    int capacidad = 0;
                    string tipo = "Desconocido";
                    List<tarifas> tarifas = new List<tarifas>();

                    //Empieza
                    Console.WriteLine(node.Name);
                    XmlNode temporal = node.ChildNodes[0];
                    Console.WriteLine(temporal.Name);

                    //Obtener el nombre del parking
                    if (temporal.ChildNodes[0] != null)
                    {
                        nombreParking =
                            temporal.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0]
                                .InnerText;
                        Console.WriteLine("Nombre: " + nombreParking);
                    }

                    //Obtener la Longitud/Latitud del parking
                    temporal = node.ChildNodes[1];
                    if (temporal.ChildNodes[0] != null)
                    {
                        string latlong = temporal.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText;
                        string[] aLatLong = latlong.Split(' ');

                        double longitud = double.Parse(aLatLong[0], CultureInfo.InvariantCulture);
                        double latitud = double.Parse(aLatLong[1], CultureInfo.InvariantCulture);

                        Console.WriteLine("Longitud " + longitud);
                        Console.WriteLine("Latitud " + latitud);

                        c.longitud = longitud;
                        c.latitud = latitud;
                    }


                    //Obtener las entradas al parking
                    int index = 2;
                    int numeroEntrada = 1;
                    while (node.ChildNodes[index].Name.Equals("edi:entrances"))
                    {
                        string latlongentr =
                            node.ChildNodes[index].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0]
                                .ChildNodes[0].InnerText;
                        string[] aLatLongentr = latlongentr.Split(' ');

                        double longitud = double.Parse(aLatLongentr[0], CultureInfo.InvariantCulture);
                        double latitud = double.Parse(aLatLongentr[1], CultureInfo.InvariantCulture);

                        entradas entrada = new entradas();

                        entrada.nombre = "Entrada " + numeroEntrada;
                        Console.WriteLine(entrada.nombre);
                        entrada.latitud = latitud;
                        entrada.longitud = longitud;
                        entradas.Add(entrada);

                        index++;
                        numeroEntrada++;
                    }

                    //Obtener el id de este Parking y tipo. Ademas, obtener tarifas ( en los que especifiquen ) >>> Segun lo observado, solo se especifica si es roadside (al borde de la carrertera)
                    string idtemp = node.ChildNodes[index].InnerText;
                    int id = int.Parse(idtemp);
                    index++;

                    //Obtener Capacidad, Tipo de parking
                    string capacidadS = node.ChildNodes[index].ChildNodes[0].ChildNodes[0].InnerText;
                    capacidad = int.Parse(capacidadS);
                    tipo = node.ChildNodes[index].ChildNodes[0].ChildNodes[1].InnerText;
                    Console.WriteLine(capacidad + " y " + tipo);
                    if (tipo.Equals("underground"))
                    {
                        tipo = "Subterraneo";
                    }
                    else
                    {
                        tipo = "Al borde de la carretera";
                    }

                    int indice = 2;
                    while (node.ChildNodes[index].ChildNodes[0].ChildNodes[indice] != null)
                    {
                        XmlNode tempN = node.ChildNodes[index].ChildNodes[0].ChildNodes[indice];
                        tempN = tempN.ChildNodes[0];

                        //Obtener los días
                        string dias = tempN.ChildNodes[0].InnerText;

                        if (dias.Equals("mondayToSaturday"))
                        {
                            dias = "Tarifa válida de Lunes a Sábado / Fare is valid from Monday to Saturday";
                        }
                        else
                        {
                            dias = "No hay datos disponibles / Data unavailable";
                        }

                        //Obtener descripción de la tarifa
                        string descripción = tempN.ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText;
                        if (descripción.Equals("Street parking tariff group"))
                        {
                            descripción =
                                "La tarifa que se aplica es la del aparcamiento en la calle / The tariff that applies is the one of parking spaces in the street";
                        }
                        else
                        {
                            descripción = "No hay datos disponibles / Data unavailable";
                        }

                        //Ultima actualizacion
                        string fecha = tempN.ChildNodes[2].InnerText;
                        int ano = int.Parse(fecha.Substring(0, 4));
                        int mes = int.Parse(fecha.Substring(5, 2));
                        int dia = int.Parse(fecha.Substring(8, 2));
                        int hora = int.Parse(fecha.Substring(11, 2));
                        int minutos = int.Parse(fecha.Substring(14, 2));
                        int segundos = int.Parse(fecha.Substring(17, 2));
                        DateTime ne = new DateTime(ano, mes, dia, hora, minutos, segundos);


                        tarifas t = new tarifas();
                        t.tipo = tipo;
                        t.descripcion = descripción;
                        t.zona = "Desconocida";
                        t.actualizacion = ne;
                        tarifas.Add(t);

                        indice++;
                    }

                    // Añadimos al parking en cuestion
                    /*string nombreParking = "Desconocido";
                    Coordenadas c = new Coordenadas();
                    LinkedList<Entrada> entradas = new LinkedList<Entrada>();
                    int capacidad = 0;
                    string tipo = "Desconocido";*/
                    parkings p = parkingslist[id];
                    if (p != null)
                    {
                        Console.WriteLine("Se ha encontrado. ID= " + p.id);
                        p.latitud = c.latitud;
                        p.longitud = c.longitud;
                        p.nombre = nombreParking;
                        p.entradas = entradas;
                        p.capacidad = capacidad;
                        p.tipo = tipo;
                        p.tarifas = tarifas;
                    }
                    else
                    {
                        Console.WriteLine(">>>>>>NO se ha encontrado. ID= " + p.id);
                    }
                }
                Console.WriteLine("Acabo Foreach Estatico");
            }
            else
            {
               Console.WriteLine("No se ha podido obtener los recintos de aparcamiento. Compruebe su conexión a internet.");
            }
            
        }

        public void paradasAutobusesBilbo()
        {
            if (paradasBilbobus.Count != 0)
            {
                paradasBilbobus.Clear();
            }

            Console.WriteLine("Empiezo BilboBus");
            XmlDocument bilbobus = this.descargaDeURL("http://www.geobilbao.net/wfsCocities?service=wfs&version=1.1.0&request=getFeature&typeName=ept:StopPoint");
            if (bilbobus != null)
            {
                XmlNodeList temp = bilbobus.SelectNodes("//*");

                //Con esto obtengo las PARADAS unicamente
                foreach (XmlNode node in temp[0].ChildNodes[0].ChildNodes)
                {
                    //Obtener id inicial
                    int id = int.Parse(node.Attributes[0].InnerText);

                    //Obtener posición geográfica
                    string latlong = node.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText;
                    string[] aLatLong = latlong.Split(' ');

                    double longitud = double.Parse(aLatLong[0], CultureInfo.InvariantCulture);
                    double latitud = double.Parse(aLatLong[1], CultureInfo.InvariantCulture);

                    //Obtener ID de la parada
                    int idParada = int.Parse(node.ChildNodes[1].ChildNodes[0].ChildNodes[0].InnerText);

                    //Obtener nombre completo
                    string nombreCompleto = node.ChildNodes[2].ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText;

                    //Obtener nombre Abreviado
                    string nombreAbreviado = node.ChildNodes[3].ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText;

                    paradas_bilbobus p = new paradas_bilbobus();
                    p.id = idParada;
                    p.latitud = latitud;
                    p.longitud = longitud;
                    p.nombre = nombreCompleto;
                    p.abreviatura = nombreAbreviado;
                    paradasBilbobus.Add(idParada, p);

                }
            }
           

        }

        private void paradasAutobusesBizkaia()
        {
            if (paradasBizkaibus.Count != 0)
            {
                paradasBizkaibus.Clear();
            }

            Console.WriteLine("Empiezo Bizkaibus");
            FTP cliente = new FTP("ftp://ftp.geo.euskadi.net/cartografia/Transporte/Moveuskadi/", "", "");
            cliente.download("Bizkaibus/google_transit.zip", @"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ReceptorBus\BizkaibusFTP\google_transit.zip");

            Console.WriteLine("¡Completado!");

            //Procesar los archivos
            Console.WriteLine("Extranyendo...");

            ZipFile zf = null;
            try
            {
                //Establezco el fichero a descomprimir
                FileStream fs = File.OpenRead(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ReceptorBus\BizkaibusFTP\google_transit.zip");
                try
                {
                    zf = new ZipFile(fs);
                }
                catch (System.ArgumentException e)
                {
                    Console.WriteLine("El fichero está dañado o la descarga no se ha realizado correctamente.");
                }
                catch(ICSharpCode.SharpZipLib.Zip.ZipException ex)
                {
                    Console.WriteLine("No se ha podido obtener las paradas de Bizkaibus. Compruebe su conexión a internet.");
                    zf = null;
                }

                if(zf!=null)
                {
                    //Por cada archivo dentro del zip, voy descomprimiendo
                    foreach (ZipEntry zipEntry in zf)
                    {
                        //Aunque no habrá directorios en nuestro caso, si los hubiera, los ignora
                        if (!zipEntry.IsFile)
                        {
                            continue;
                        }
                        //Obtiene el nombre del fichero que contiene el zip
                        String entryFileName = zipEntry.Name;


                        byte[] buffer = new byte[4096];
                        Stream zipStream = zf.GetInputStream(zipEntry);

                        // Manipulate the output filename here as desired.
                        String fullZipToPath = Path.Combine(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ReceptorBus\BizkaibusFTP\", entryFileName);
                        string directoryName = Path.GetDirectoryName(fullZipToPath);
                        if (directoryName.Length > 0)
                            Directory.CreateDirectory(directoryName);

                        using (FileStream streamWriter = File.Create(fullZipToPath))
                        {
                            StreamUtils.Copy(zipStream, streamWriter, buffer);
                        }
                    }
                }
                
            }
            finally
            {
                if (zf != null)
                {
                    zf.IsStreamOwner = true; // Makes close also shut the underlying stream
                    zf.Close(); // Ensure we release resources
                }
            }

            if (zf != null)
            {
                Console.WriteLine("¡Completado!");

                Console.WriteLine("Leyendo ficheros...");

                int counter = 0;
                string line;

                // Leemos el fichero linea por linea
                System.IO.StreamReader file =
                   new System.IO.StreamReader(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ReceptorBus\BizkaibusFTP\stops.txt");
                string primeraLinea = file.ReadLine();
                while ((line = file.ReadLine()) != null)
                {
                    int id;
                    int codigo;
                    string nombreParada;
                    string descripcion;
                    Coordenadas c;
                    string urlParada;
                    string tipoLocalizacion;
                    string idParadaPadre;

                    string[] valoresLinea = line.Split(',');

                    //Obtener el id de la parada
                    if (!valoresLinea[0].Equals(""))
                    {
                        id = int.Parse(valoresLinea[0]);

                        //Obtener codigo de la parada
                        codigo = int.Parse(valoresLinea[1]);


                        //Obtener nombre
                        nombreParada = valoresLinea[2];

                        //descripcion
                        descripcion = valoresLinea[3];

                        //latitud y longitud
                        if (valoresLinea[4].Equals(""))
                        {
                            c = new Coordenadas(double.Parse(valoresLinea[5], CultureInfo.InvariantCulture), double.Parse(valoresLinea[6], CultureInfo.InvariantCulture));
                            descripcion = valoresLinea[2] + valoresLinea[3];
                        }
                        else
                        {
                            c = new Coordenadas(double.Parse(valoresLinea[4], CultureInfo.InvariantCulture), double.Parse(valoresLinea[5], CultureInfo.InvariantCulture));
                        }


                        //Url de parada
                        urlParada = valoresLinea[7];

                        //tipoLocalizacion
                        tipoLocalizacion = valoresLinea[8];

                        //ID de la parada padre
                        idParadaPadre = valoresLinea[9];

                        //Añadimos a la lista de paradas de autobus de Bizkaibus
                        paradas_bizkaibus p = new paradas_bizkaibus();
                        p.id = id;
                        p.codigoParada = codigo;
                        p.nombre = nombreParada;
                        p.descripcion = descripcion;
                        p.latitud = c.latitud;
                        p.longitud = c.longitud;
                        p.url = urlParada;
                        try
                        {
                            p.tipoLocalizacion = int.Parse(tipoLocalizacion);
                        }
                        catch (FormatException ex)
                        {
                            p.tipoLocalizacion = 0;
                        }

                        try
                        {
                            p.idParadaPadre = int.Parse(idParadaPadre);
                        }
                        catch (FormatException ex)
                        {
                            p.idParadaPadre = 0;
                        }
                        paradasBizkaibus.Add(id, p);
                    }
                    
                }

                file.Close();

                Console.WriteLine("¡Completado datos!");

                
            }
            

        }

        private void farmacias()
        {
            if (farmaciasList.Count != 0)
            {
                farmaciasList.Clear();
            }

            Console.WriteLine("Empiezo Farmacias");

            XmlDocument farmaciasxml = this.descargaDeURL("http://opendata.euskadi.eus/contenidos/ds_localizaciones/farmacias_de_euskadi/opendata/farmacias.xml");
            int totalMal = 0;
            if (farmaciasxml != null)
            {
                XmlNodeList temp = farmaciasxml.SelectNodes("//*");
                foreach (XmlNode node in temp[0].ChildNodes)
                {
                    //Nombre del propietario (¿?entiendo)
                    string nombreDocumental = node.ChildNodes[0].InnerText;
                    //Direccion en la que se encuentra (completa)
                    string direccionCompleta = node.ChildNodes[1].InnerText;
                    //Nombre de la farmacia
                    string nombreFarmacia = node.ChildNodes[4].InnerText;
                    //Codigo postal
                    int codigoPostal = 00000;
                    if(node.ChildNodes[4].Name.Equals("sanidadpostalcode"))
                    {
                        if (!node.ChildNodes[4].InnerText.Equals(""))
                        {
                            codigoPostal = int.Parse(node.ChildNodes[4].InnerText);
                        }
                    }
                    else
                    {
                        if (node.ChildNodes[5].Name.Equals("sanidadpostalcode"))
                        {
                            if (!node.ChildNodes[5].InnerText.Equals(""))
                            {
                                codigoPostal = int.Parse(node.ChildNodes[5].InnerText);
                            }
                        }
                    }

                    //Provincia
                    string provincia = "Desconocida";
                    if(node.ChildNodes[4].Name.Equals("sanidadprovince"))
                    {
                        provincia = node.ChildNodes[4].InnerText;
                    }
                    else
                    {
                        if(node.ChildNodes[5].Name.Equals("sanidadprovince"))
                        {
                            provincia = node.ChildNodes[5].InnerText;
                        }
                        else
                        {
                            provincia = node.ChildNodes[6].InnerText;
                        }
                    }

                    //Direccion en la que se encuentra (abreviada)
                    string dirAbreviada = "No especificada";
                    if (node.ChildNodes[7].InnerText.Equals(""))
                    {
                        if (node.ChildNodes[8].InnerText.Equals(""))
                        {
                            if (!node.ChildNodes[9].InnerText.Equals(""))
                            {
                                dirAbreviada = node.ChildNodes[9].InnerText;
                            }
                        }
                        else
                        {
                            dirAbreviada = node.ChildNodes[8].InnerText;
                        }
                    }
                    else
                    {
                        dirAbreviada = node.ChildNodes[7].InnerText;
                    }

                    //Ciudad donde esta la farmacia
                    string ciudad = node.ChildNodes[8].InnerText;

                    //Url con más información (puede ser NULL)
                    string urlInfo = "Desconocida";
                    if (node.ChildNodes[7].Name.Equals("physicalurl"))
                    {
                        urlInfo = node.ChildNodes[7].InnerText;
                    }
                    else
                    {
                        if (node.ChildNodes[8].Name.Equals("physicalurl"))
                        {
                            urlInfo = node.ChildNodes[8].InnerText;
                        }
                        else
                        {
                            urlInfo = node.ChildNodes[9].InnerText;
                        }
                    }

                    //Ahora pasaremos a obtener más información de la Farmacia, pero desde otro XML enlazado a este
                    XmlDocument masInfo = null;
                    if (node.ChildNodes[8].Name.Equals("dataxml"))
                    {
                        masInfo = this.descargaDeURL(node.ChildNodes[8].InnerText);
                    }
                    else
                    {
                        if (node.ChildNodes[9].Name.Equals("dataxml"))
                        {
                            masInfo = this.descargaDeURL(node.ChildNodes[9].InnerText);
                        }
                        else
                        {
                            if (node.ChildNodes[10].Name.Equals("dataxml"))
                            {
                                masInfo = this.descargaDeURL(node.ChildNodes[10].InnerText);
                            }
                            else
                            {
                                if (node.ChildNodes[11].Name.Equals("dataxml"))
                                {
                                    masInfo = this.descargaDeURL(node.ChildNodes[11].InnerText);
                                }
                                else
                                {
                                    if (node.ChildNodes[12].Name.Equals("dataxml"))
                                    {
                                        masInfo = this.descargaDeURL(node.ChildNodes[12].InnerText);
                                    }
                                    else
                                    {
                                        masInfo = this.descargaDeURL(node.ChildNodes[13].InnerText);
                                    }
                                }
                            }
                        }
                    }
                    
                    long telefono = 00000000;
                    long fax = 000000000;
                    double latitud = 0.0;
                    double longitud = 0.0;
                    string email = "Desconocido";
                    string web = "Desconocida";
                    string adicional = "No disponible";
                    if (masInfo != null)
                    {
                        if (masInfo.HasChildNodes)
                        {
                            //Obtengo información como la latitud y longitud, el teléfono o web/email si tiene
                            Console.WriteLine(masInfo.ChildNodes[1].Name);

                            //Localización
                            if (!masInfo.ChildNodes[1].ChildNodes[4].ChildNodes[6].ChildNodes[0].InnerText.Equals(""))
                            {
                                latitud = double.Parse(masInfo.ChildNodes[1].ChildNodes[4].ChildNodes[6].ChildNodes[0].InnerText, CultureInfo.InvariantCulture);
                                longitud = double.Parse(masInfo.ChildNodes[1].ChildNodes[4].ChildNodes[6].ChildNodes[1].InnerText, CultureInfo.InvariantCulture);
                            }
                            
                            
                            //Información de contacto
                            //Telefono (si tiene)

                            if (!masInfo.ChildNodes[1].ChildNodes[5].ChildNodes[0].InnerText.Equals(""))
                            {
                                telefono = long.Parse(masInfo.ChildNodes[1].ChildNodes[5].ChildNodes[0].InnerText.Replace(" ", ""));
                            }

                            //Web (si tiene)

                            if (!masInfo.ChildNodes[1].ChildNodes[5].ChildNodes[4].InnerText.Equals(""))
                            {
                                web = masInfo.ChildNodes[1].ChildNodes[5].ChildNodes[4].InnerText;
                            }
                        }
                    }
                    else
                    {
                        totalMal++;
                    }

                    Almacenamiento.farmacias f = new farmacias();
                    f.nombrePropietario = nombreDocumental;
                    f.direccion = direccionCompleta;
                    f.nombre = nombreFarmacia;
                    f.codigoPostal = codigoPostal;
                    f.provincia = provincia;
                    f.direccionAbreviada = dirAbreviada;
                    f.ciudad = ciudad;
                    f.url = urlInfo;
                    f.latitud = latitud;
                    f.longitud = longitud;
                    f.telefono = telefono;
                    f.web = web;
                    f.informacionAdcional = "";
                    
                    farmaciasList.Add(f);

                    if (fichero != null)
                    {
                        fichero.Dispose();
                    }
                }
            }
            else
            {
                Console.WriteLine("No se ha podido obtener las farmacias. Compruebe su conexión a internet.");
            }
            

            //Indica cuantas URLs están mal
            Console.WriteLine("Hay " + totalMal + " URL/XML mal.");
            farmaciasxml = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
           

        }

        private void hospitales()
        {
            if (hospitalList.Count != 0)
            {
                hospitalList.Clear();
            }

            Console.WriteLine("Empiezo Hospitales");
            XmlDocument aparcamientosxml = this.descargaDeURL("http://opendata.euskadi.eus/contenidos/ds_localizaciones/hospitales_en_euskadi/opendata/hospitales.xml");

            int totalMal = 0;
            if (aparcamientosxml != null)
            {
                XmlNodeList temp = aparcamientosxml.SelectNodes("//*");
                //Con esto obtengo los HOSPITALES
                foreach (XmlNode node in temp[0].ChildNodes)
                {
                    //Nombre del hospital
                    string nombreHospital = node.ChildNodes[0].InnerText;

                    //Direccion completa del hospital
                    string dirCompleta = node.ChildNodes[1].InnerText;

                    //Identificativo/Codigo de Hospital
                    string codigoDeHospital = node.ChildNodes[3].InnerText;

                    //Codigo postal
                    int codigoPostal = int.Parse(node.ChildNodes[5].InnerText);

                    //Region
                    string region = node.ChildNodes[6].InnerText;

                    //La calle del hospital
                    string calle = node.ChildNodes[9].InnerText;

                    //Ciudad
                    string ciudad = "Desconocida";
                    if (node.ChildNodes[9].Name.Equals("sanidadtown"))
                    {
                        ciudad = node.ChildNodes[9].InnerText;
                    }
                    else
                    {
                        ciudad = node.ChildNodes[10].InnerText;
                    }

                    //Obtener info extra
                    //De la URL que se nos da
                    XmlDocument masInfo = null;
                    if(node.ChildNodes[11].Name.Equals("dataxml"))
                    {
                        masInfo = this.descargaDeURL(node.ChildNodes[11].InnerText);
                    }
                    else
                    {
                        if (node.ChildNodes[12].Name.Equals("dataxml"))
                        {
                            masInfo = this.descargaDeURL(node.ChildNodes[12].InnerText); 
                        }
                        else
                        {
                            if (node.ChildNodes[13].Name.Equals("dataxml"))
                            {
                                masInfo = this.descargaDeURL(node.ChildNodes[13].InnerText);
                            }
                            else
                            {
                                masInfo = this.descargaDeURL(node.ChildNodes[14].InnerText);
                            }
                        }
                        
                    }
                    double latitud;
                    double longitud;
                    string email = "Desconocido";
                    string web = "Desconocida";
                    long telefono = 000000000;
                    long fax = 000000000;
                    if (masInfo == null)
                    {
                        Console.WriteLine("No ha sido posible obtener el XML del Hospital: " + nombreHospital);
                        Console.WriteLine("Insercción directa");
                        if (nombreHospital.Equals("Hospital Psiquiátrico de Álava"))
                        {
                            Console.WriteLine("Alava");
                            latitud = 42.83611512748661;
                            longitud = -2.67895289999513;
                            email = "secretariagerencia.sma@osakidetza.net";
                            web = "http://www.osakidetza.euskadi.net/r85-ghhpsq00/es/";
                            telefono = 945006555;
                            fax = 945006587;

                        }
                        else if (nombreHospital.Equals("Hospital Psiquiátrico de Zaldibar"))
                        {
                            Console.WriteLine("Zaldibar");
                            latitud = 43.1698592;
                            longitud = -2.5494593;
                            email = "hospital.zaldibar@hzal.osakidetza.net";
                            web = "Desconocida";
                            telefono = 946032800;
                            fax = 946032819;
                            calle = "Avda. Bilbao, S/N";
                        }
                        else
                        {
                            Console.WriteLine("No funciona");
                            latitud = 0.0;
                            longitud = 0.0;
                        }
                        totalMal++;

                    }
                    else
                    {
                        if (nombreHospital.Equals("Hospital de Mendaro"))
                        {
                            longitud = double.Parse(
                            masInfo.ChildNodes[1].ChildNodes[4].ChildNodes[6].ChildNodes[0].InnerText,
                            CultureInfo.InvariantCulture);
                            latitud =
                                double.Parse(masInfo.ChildNodes[1].ChildNodes[4].ChildNodes[6].ChildNodes[1].InnerText,
                                    CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            //latitud y longitud
                            latitud = double.Parse(
                                masInfo.ChildNodes[1].ChildNodes[4].ChildNodes[6].ChildNodes[0].InnerText,
                                CultureInfo.InvariantCulture);
                            longitud =
                                double.Parse(masInfo.ChildNodes[1].ChildNodes[4].ChildNodes[6].ChildNodes[1].InnerText,
                                    CultureInfo.InvariantCulture); 
                        }

                        //telefono, web
                        if (!masInfo.ChildNodes[1].ChildNodes[5].ChildNodes[0].InnerText.Equals(""))
                        {
                            telefono = long.Parse(masInfo.ChildNodes[1].ChildNodes[5].ChildNodes[0].InnerText);
                        }

                        //Web (si tiene)

                        if (masInfo.ChildNodes[1].ChildNodes[5].ChildNodes[3].Name.Equals("web") && !masInfo.ChildNodes[1].ChildNodes[5].ChildNodes[3].InnerText.Equals(""))
                        {
                            web = masInfo.ChildNodes[1].ChildNodes[5].ChildNodes[3].InnerText;
                        }
                        else
                        {
                            web = masInfo.ChildNodes[1].ChildNodes[5].ChildNodes[4].InnerText;
                        }

                    }

                    Almacenamiento.hospitales h = new hospitales();
                    h.nombre = nombreHospital;
                    h.direccion = dirCompleta;
                    h.idHospital = codigoDeHospital;
                    h.codigoPostal = codigoPostal;
                    h.region = region;
                    h.calle = calle;
                    h.ciudad = ciudad;
                    h.latitud = latitud;
                    h.longitud = longitud;
                    h.web = web;
                    h.telefono = telefono;

                    //Añado a la lista
                    this.hospitalList.Add(h);

                }
            }
            else
            {
                Console.WriteLine("No se ha podido obtener los hospitales. Compruebe su conexión a internet.");
            }
            
            //Indica cuantas URLs están mal
            Console.WriteLine("Hay " + totalMal + " URL/XML mal.");
           


        }

        private void centrosDeSalud()
        {
            if (centroSaludList.Count != 0)
            {
                centroSaludList.Clear();
            }

            Console.WriteLine("Empiezo Centros de Salud");
            XmlDocument centrosdesaludxml = this.descargaDeURL("http://opendata.euskadi.eus/contenidos/ds_localizaciones/centros_salud_en_euskadi/opendata/centros-salud.xml");
            int totalMal = 0;
            if (centrosdesaludxml != null)
            {
                XmlNodeList temp = centrosdesaludxml.SelectNodes("//*");

                //Con esto obtengo los CENTROS DE SALUD
                foreach (XmlNode node in temp[0].ChildNodes)
                {
                    //Nombre del hospital
                    string nombreCentro = node.ChildNodes[0].InnerText;

                    //Direccion completa del hospital
                    string dirCompleta = node.ChildNodes[1].InnerText;

                    //Identificativo/Codigo del Centro de salud
                    string codigoCentro = "Desconocido";
                    if (node.ChildNodes[4].Name.Equals("sanidadcode"))
                    {
                        codigoCentro = node.ChildNodes[4].InnerText;
                    }
                    else
                    {
                        if (node.ChildNodes[3].Name.Equals("sanidadcode"))
                        {
                            codigoCentro = node.ChildNodes[3].InnerText;
                        }
                    }


                    //Codigo postal
                    int codigoPostal = 00000;
                    int index = 7;
                    if (node.ChildNodes[6].InnerText.Equals("NULL"))
                    {
                        codigoPostal = 00000;
                    }
                    else
                    {
                        
                        if (node.ChildNodes[6].Name.Equals("sanidadpostalcode"))
                        {
                            codigoPostal = int.Parse(node.ChildNodes[6].InnerText.Trim());
                        }
                        else
                        {
                            if (node.ChildNodes[5].Name.Equals("sanidadpostalcode"))
                            {
                                if (!node.ChildNodes[5].InnerText.Equals("NULL"))
                                {
                                    codigoPostal = int.Parse(node.ChildNodes[5].InnerText.Trim());
                                }
                                index = 6;
                            }
                        }
                    }


                    //Provincia
                    string provincia = node.ChildNodes[index].InnerText;

                    index++;

                    //Region
                    string region = node.ChildNodes[index].InnerText;

                    //Horario
                    string horario = "", calle = "", ciudad = "", url = "";
                    XmlDocument masInfo = null;

                    if (node.ChildNodes[11].Name.Equals("sanidadtimetable"))
                    {
                        string temporal = node.ChildNodes[11].InnerText;
                        if (!temporal.Equals(""))
                        {
                            temporal = temporal.Substring(9);
                            horario = temporal.Replace("&lt;/p&gt;", "");
                            Console.WriteLine(horario);
                        }
                        else
                        {
                            horario = "Desconocido";
                        }

                        calle = node.ChildNodes[10].InnerText;
                        ciudad = node.ChildNodes[12].InnerText;
                        url = node.ChildNodes[13].InnerText;
                        //Obtener info extra
                        //De la URL que se nos da
                        masInfo = this.descargaDeURL(node.ChildNodes[15].InnerText);
                    }
                    else
                    {
                        if (node.ChildNodes[10].Name.Equals("sanidadtimetable"))
                        {
                            string temporal = node.ChildNodes[10].InnerText;
                            if (!temporal.Equals(""))
                            {
                                temporal = temporal.Substring(9);
                                horario = temporal.Replace("&lt;/p&gt;", "");
                                Console.WriteLine(horario);
                            }
                            else
                            {
                                horario = "Desconocido";
                            }

                            calle = node.ChildNodes[9].InnerText;
                            ciudad = node.ChildNodes[11].InnerText;
                            url = node.ChildNodes[12].InnerText;
                            //Obtener info extra
                            //De la URL que se nos da
                            masInfo = this.descargaDeURL(node.ChildNodes[14].InnerText);
                        }
                        else
                        {
                            if (node.ChildNodes[12].Name.Equals("sanidadtimetable"))
                            {
                                string temporal = node.ChildNodes[12].InnerText;
                                if (!temporal.Equals(""))
                                {
                                    temporal = temporal.Substring(9);
                                    horario = temporal.Replace("&lt;/p&gt;", "");
                                    Console.WriteLine(horario);
                                }
                                else
                                {
                                    horario = "Desconocido";
                                }

                                calle = node.ChildNodes[11].InnerText;
                                ciudad = node.ChildNodes[13].InnerText;
                                url = node.ChildNodes[14].InnerText;
                                
                            }
                        }
                    }

                    //Obtener info extra
                    //De la URL que se nos da
                    if(node.ChildNodes[10].Name.Equals("dataxml"))
                    {
                        masInfo = this.descargaDeURL(node.ChildNodes[10].InnerText);
                    }
                    else
                    {
                        if (node.ChildNodes[11].Name.Equals("dataxml"))
                        {
                            masInfo = this.descargaDeURL(node.ChildNodes[11].InnerText);
                        }
                        else
                        {
                            if (node.ChildNodes[12].Name.Equals("dataxml"))
                            {
                                masInfo = this.descargaDeURL(node.ChildNodes[12].InnerText);
                            }
                            else
                            {
                                if (node.ChildNodes[13].Name.Equals("dataxml"))
                                {
                                    masInfo = this.descargaDeURL(node.ChildNodes[13].InnerText);
                                }
                                else
                                {
                                    if (node.ChildNodes[14].Name.Equals("dataxml"))
                                    {
                                        masInfo = this.descargaDeURL(node.ChildNodes[14].InnerText);
                                    }
                                    else
                                    {
                                        if (node.ChildNodes[15].Name.Equals("dataxml"))
                                        {
                                            masInfo = this.descargaDeURL(node.ChildNodes[15].InnerText);
                                        }
                                        else
                                        {
                                            if (node.ChildNodes[16].Name.Equals("dataxml"))
                                            {
                                                masInfo = this.descargaDeURL(node.ChildNodes[16].InnerText);
                                            }
                                            else
                                            {
                                                masInfo = this.descargaDeURL(node.ChildNodes[17].InnerText);
                                            }
                                        }

                                    }
                                }
                            }
                        }
                    }
                    
                    

                    double latitud = 0.0;
                    double longitud = 0.0;
                    string email = "Desconocido";
                    string web = "Desconocida";
                    long telefono = 000000000;
                    long fax = 000000000;

                    if (masInfo == null)
                    {
                        Console.WriteLine("No ha sido posible obtener el XML del Hospital: " + nombreCentro);
                        totalMal++;
                    }
                    else
                    {
                        // Console.WriteLine(masInfo.ChildNodes[1].Name);

                        //latitud y longitud
                        if (!masInfo.ChildNodes[1].ChildNodes[4].ChildNodes[6].ChildNodes[0].InnerText.Equals(""))
                        {
                            latitud =
                                double.Parse(
                                    masInfo.ChildNodes[1].ChildNodes[4].ChildNodes[6].ChildNodes[0].InnerText.Trim(),
                                    CultureInfo.InvariantCulture);
                        }

                        if (!masInfo.ChildNodes[1].ChildNodes[4].ChildNodes[6].ChildNodes[0].InnerText.Equals(""))
                        {
                            longitud =
                                double.Parse(
                                    masInfo.ChildNodes[1].ChildNodes[4].ChildNodes[6].ChildNodes[0].InnerText.Trim(),
                                    CultureInfo.InvariantCulture);
                        }

                        //telefono
                        if (!masInfo.ChildNodes[1].ChildNodes[5].ChildNodes[0].InnerText.Equals(""))
                        {
                            if (masInfo.ChildNodes[1].ChildNodes[5].ChildNodes[0].InnerText.Length > 9)
                            {
                                telefono =
                                    long.Parse(masInfo.ChildNodes[1].ChildNodes[5].ChildNodes[0].InnerText.Substring(0,
                                        9).Replace(" ", ""));
                            }
                            else
                            {
                                telefono = long.Parse(masInfo.ChildNodes[1].ChildNodes[5].ChildNodes[0].InnerText);
                            }

                        }

                        //Web (si tiene)

                        if (!masInfo.ChildNodes[1].ChildNodes[5].ChildNodes[4].InnerText.Equals(""))
                        {
                            web = masInfo.ChildNodes[1].ChildNodes[5].ChildNodes[4].InnerText;
                        }

                        Console.WriteLine(latitud + " " + longitud + " " + telefono + " " + fax + " " + email + " " +
                                          web);
                    }

                    //Añado a la lista
                    centros_de_salud varTemporal = new centros_de_salud();
                    varTemporal.nombre = nombreCentro;
                    varTemporal.direcionCompleta = dirCompleta;
                    varTemporal.codigo_centro = codigoCentro;
                    varTemporal.codigoPostal = codigoPostal;
                    varTemporal.provincia = provincia;
                    varTemporal.region = region;
                    varTemporal.horario = horario;
                    varTemporal.calle = calle;
                    varTemporal.ciudad = ciudad;
                    varTemporal.urlAdicional = url;
                    varTemporal.latitud = latitud;
                    varTemporal.longitud = longitud;
                    varTemporal.web = web;
                    varTemporal.telefono = telefono;
                    this.centroSaludList.Add(varTemporal);


                }
            }
            else
            {
                Console.WriteLine("No se ha podido obtener los centros de salud. Compruebe su conexión a internet.");
            }

            //Indica cuantas URLs están mal
            Console.WriteLine("Hay " + totalMal + " URL/XML mal.");

        }

       
        
        //Metodo para obtener la información de los puntos para bicicletas
        private void bicicletas()
        {
            if (puntosBicisList.Count != 0)
            {
                puntosBicisList.Clear();
            }

            Console.WriteLine("Empiezo Bicicletas");

            XmlDocument bicicletasxml = this.descargaDeURL("http://www.bilbao.net/WebServicesBilbao/WSBilbao?s=ODPRESBICI&u=OPENDATA&p0=A&p1=A");

            if (bicicletasxml != null)
            {
                XmlNodeList temp = bicicletasxml.SelectNodes("//*");
                foreach (XmlNode node in temp[0].ChildNodes[1].ChildNodes)
                {
                    //ID de punto de bicicletas
                    int id = int.Parse(node.ChildNodes[0].InnerText);

                    //Nombre de punto de bicicletas
                    string nombre = node.ChildNodes[1].InnerText;

                    //Estado de punto de bicicletas
                    string estado = node.ChildNodes[2].InnerText;

                    //Coordenadas de punto de bicicletas
                    double latitud = double.Parse(node.ChildNodes[3].InnerText, CultureInfo.InvariantCulture);
                    double longitud = double.Parse(node.ChildNodes[4].InnerText, CultureInfo.InvariantCulture);


                    //Anclajes libres
                    int anclajesLibres = int.Parse(node.ChildNodes[5].InnerText);

                    //Anclajes averiados
                    int anclajesAveriados = int.Parse(node.ChildNodes[6].InnerText);

                    //Anclajes usados
                    int anclajesUsados = int.Parse(node.ChildNodes[7].InnerText);

                    //Bicis Libres
                    int bicisLibres = int.Parse(node.ChildNodes[8].InnerText);

                    //Bicis averiadas
                    int bicisAveriadas = int.Parse(node.ChildNodes[9].InnerText);

                    puntos_bici p = new puntos_bici();
                    p.id = id;
                    p.nombre = nombre;
                    p.estado = estado;
                    p.latitud = latitud;
                    p.longitud = longitud;
                    p.capacidad = anclajesLibres + anclajesUsados + anclajesAveriados;

                    puntosBicisList.Add(p);
                }
            }
            else
            {
                Console.WriteLine("No se ha podido obtener la información de bicicletas. Compruebe su conexión a internet.");
            }

        }

        /*private void parkingDeusto()
        {
            Console.WriteLine("Empiezo Aparcamiento Deusto");
            DeustoParkingServiceClient client = new DeustoParkingServiceClient();
            contadorDBSDeusto = client.GetLastActivity().DBSCounter;
            contadorGeneralDeusto = client.GetLastActivity().GRALCounter;
            Console.WriteLine("Número de plazas libres en DBS: "+ contadorDBSDeusto+ " / Número de plazas libres en General: "+ contadorGeneralDeusto);
            
        }*/

        private void metroBilbao()
        {
            if (paradasMetro.Count != 0)
            {
                lineasMetroBilbao.Clear();
            }

            Console.WriteLine("Empiezo Metro");
            FTP cliente = new FTP("ftp://ftp.geo.euskadi.net/cartografia/Transporte/Moveuskadi/", "", "");
            cliente.download("MetroBilbao/google_transit.zip", @"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ReceptorBus\MetroFTP\google_transit.zip");

            Console.WriteLine("¡Completado!");

            //Procesar los archivos
            Console.WriteLine("Extranyendo...");

            ZipFile zf = null;
            try
            {
                //Establezco el fichero a descomprimir
                FileStream fs = File.OpenRead(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ReceptorBus\MetroFTP\google_transit.zip");
                try
                {
                    zf = new ZipFile(fs);
                }
                catch (System.ArgumentException e)
                {
                    Console.WriteLine("El fichero está dañado o la descarga no se ha realizado correctamente.");
                    zf = null;
                }

                if (zf != null)
                {
                    //Por cada archivo dentro del zip, voy descomprimiendo
                    foreach (ZipEntry zipEntry in zf)
                    {
                        //Aunque no habrá directorios en nuestro caso, si los hubiera, los ignora
                        if (!zipEntry.IsFile)
                        {
                            continue;
                        }
                        //Obtiene el nombre del fichero que contiene el zip
                        String entryFileName = zipEntry.Name;


                        byte[] buffer = new byte[4096];
                        Stream zipStream = zf.GetInputStream(zipEntry);

                        // Manipulate the output filename here as desired.
                        String fullZipToPath = Path.Combine(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ReceptorBus\MetroFTP\", entryFileName);
                        string directoryName = Path.GetDirectoryName(fullZipToPath);
                        if (directoryName.Length > 0)
                            Directory.CreateDirectory(directoryName);

                        using (FileStream streamWriter = File.Create(fullZipToPath))
                        {
                            StreamUtils.Copy(zipStream, streamWriter, buffer);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No se ha podido obtener las paradas de metro. Compruebe su conexión a internet.");
                }
                
            }
            finally
            {
                if (zf != null)
                {
                    zf.IsStreamOwner = true; // Makes close also shut the underlying stream
                    zf.Close(); // Ensure we release resources
                }
            }

            if (zf != null)
            {
                Console.WriteLine("¡Completado!");

                Console.WriteLine("Leyendo ficheros...");

                int counter = 0;
                string line;

                // Leemos el fichero linea por linea
                System.IO.StreamReader file =
                   new System.IO.StreamReader(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ReceptorBus\MetroFTP\stops.txt");
                string primeraLinea = file.ReadLine();
                while ((line = file.ReadLine()) != null)
                {

                    string id;
                    string codigo;
                    string nombreParada;
                    Coordenadas c;
                    string tipoLocalizacion;
                    string idParadaPadre;

                    string[] valoresLinea = line.Split(',');

                    //Obtener el id de la parada
                    id = valoresLinea[0];

                    //Obtener codigo parada
                    codigo = valoresLinea[1];

                    //Obtener nombre
                    nombreParada = valoresLinea[2];

                    //latitud y longitud
                    c = new Coordenadas(double.Parse(valoresLinea[3], CultureInfo.InvariantCulture), double.Parse(valoresLinea[4], CultureInfo.InvariantCulture));

                    //tipoLocalizacion
                    tipoLocalizacion = valoresLinea[5];

                    //ID de la parada padre
                    idParadaPadre = valoresLinea[6];

                    //Añadimos a la lista de paradas de autobus de Metro
                    paradas_metro p = new paradas_metro();
                    p.idParada = id;
                    p.codigoParada = codigo;
                    p.nombre = nombreParada;
                    p.latitud = c.latitud;
                    p.longitud = p.longitud;
                    p.tipoLocalizacion = int.Parse(tipoLocalizacion);
                    p.idParadaPadre = idParadaPadre;
                    paradasMetro.Add(id, p);

                }

                file.Close();

                //Asignar paradas padre


                Console.WriteLine("¡Completado datos!");

                
            }
            

        }

        private void euskotren()
        {
            if (paradasEuskotren.Count != 0)
            {
                paradasEuskotren.Clear();
            }

            Console.WriteLine("Empiezo Euskotren");
            FTP cliente = new FTP("ftp://ftp.geo.euskadi.net/cartografia/Transporte/Moveuskadi/", "", "");
            cliente.download("Euskotren/google_transit.zip", @"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ReceptorBus\EuskotrenFTP\google_transit.zip");

            Console.WriteLine("¡Completado!");

            //Procesar los archivos
            Console.WriteLine("Extranyendo...");

            ZipFile zf = null;
            try
            {
                //Establezco el fichero a descomprimir
                FileStream fs = File.OpenRead(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ReceptorBus\EuskotrenFTP\google_transit.zip");
                try
                {
                    zf = new ZipFile(fs);
                }
                catch (System.ArgumentException e)
                {
                    Console.WriteLine("El fichero está dañado o la descarga no se ha realizado correctamente.");
                    zf = null;
                }

                if (zf != null)
                {
                    //Por cada archivo dentro del zip, voy descomprimiendo
                    foreach (ZipEntry zipEntry in zf)
                    {
                        //Aunque no habrá directorios en nuestro caso, si los hubiera, los ignora
                        if (!zipEntry.IsFile)
                        {
                            continue;
                        }
                        //Obtiene el nombre del fichero que contiene el zip
                        String entryFileName = zipEntry.Name;


                        byte[] buffer = new byte[4096];
                        Stream zipStream = zf.GetInputStream(zipEntry);

                        // Manipulate the output filename here as desired.
                        String fullZipToPath = Path.Combine(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ReceptorBus\EuskotrenFTP\", entryFileName);
                        string directoryName = Path.GetDirectoryName(fullZipToPath);
                        if (directoryName.Length > 0)
                            Directory.CreateDirectory(directoryName);

                        using (FileStream streamWriter = File.Create(fullZipToPath))
                        {
                            StreamUtils.Copy(zipStream, streamWriter, buffer);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No se ha podido obtener las paradas de Euskotren. Compruebe su conexión a internet.");
                }
                
            }
            finally
            {
                if (zf != null)
                {
                    zf.IsStreamOwner = true; // Makes close also shut the underlying stream
                    zf.Close(); // Ensure we release resources
                }
            }

            if (zf != null)
            {
                Console.WriteLine("¡Completado!");

                Console.WriteLine("Leyendo ficheros...");

                int counter = 0;
                string line;

                // Leemos el fichero linea por linea
                System.IO.StreamReader file =
                   new System.IO.StreamReader(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ReceptorBus\EuskotrenFTP\stops.txt");
                string primeraLinea = file.ReadLine();
                while ((line = file.ReadLine()) != null)
                {

                    int id;
                    string codigo;
                    string nombreParada;
                    string descripcion;
                    string idZona;
                    Coordenadas c;
                    string tipoLocalizacion;
                    string urlParada;

                    string[] valoresLinea = line.Split(',');

                    //Obtener el id de la parada
                    id = int.Parse(valoresLinea[0]);

                    //Obtener codigo parada
                    codigo = valoresLinea[1];

                    //Obtener nombre
                    nombreParada = valoresLinea[2];

                    //Descripcion parada
                    descripcion = valoresLinea[3];

                    //latitud y longitud
                    c = new Coordenadas(double.Parse(valoresLinea[4], CultureInfo.InvariantCulture), double.Parse(valoresLinea[5], CultureInfo.InvariantCulture));

                    //id de zona
                    idZona = valoresLinea[6];

                    //url de parada
                    urlParada = valoresLinea[7];

                    //tipoLocalizacion
                    tipoLocalizacion = valoresLinea[8];

                    //Añadimos a la lista de paradas de autobus de Euskotren
                    paradas_euskotren p = new paradas_euskotren();
                    p.id = id;
                    p.codigoParada = codigo;
                    p.nombre = nombreParada;
                    p.descripcion = descripcion;
                    p.latitud = c.latitud;
                    p.longitud = c.longitud;
                    p.url = urlParada;
                    p.tipoLocalizacion = int.Parse(tipoLocalizacion.Trim());
                    paradasEuskotren.Add(id, p);


                }

                file.Close();

                Console.WriteLine("¡Completado datos!");

                
            }
            
        }

        public void lineasBilbobus()
        {
            lineasBilbo.Clear();

            Console.WriteLine("Empiezo Lineas Bilbobus");

            if (
                !File.Exists(
                    @"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ReceptorBus\BilbobusFTP\google_transit.zip"))
            {
                FTP cliente = new FTP("ftp://ftp.geo.euskadi.net/cartografia/Transporte/Moveuskadi/", "", "");
                cliente.download("Bilbobus/google_transit.zip",
                    @"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ReceptorBus\BilbobusFTP\google_transit.zip");
            }

            Console.WriteLine("¡Completado!");

            //Procesar los archivos
            Console.WriteLine("Extranyendo...");

            ZipFile zf = null;
            try
            {
                //Establezco el fichero a descomprimir
                FileStream fs = File.OpenRead(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ReceptorBus\BilbobusFTP\google_transit.zip");
                try
                {
                    zf = new ZipFile(fs);
                }
                catch (System.ArgumentException e)
                {
                    Console.WriteLine("El fichero está dañado o la descarga no se ha realizado correctamente.");
                    zf = null;
                }

                if (zf != null)
                {
                    //Por cada archivo dentro del zip, voy descomprimiendo
                    foreach (ZipEntry zipEntry in zf)
                    {
                        //Aunque no habrá directorios en nuestro caso, si los hubiera, los ignora
                        if (!zipEntry.IsFile)
                        {
                            continue;
                        }
                        //Obtiene el nombre del fichero que contiene el zip
                        String entryFileName = zipEntry.Name;


                        byte[] buffer = new byte[4096];
                        Stream zipStream = zf.GetInputStream(zipEntry);

                        // Manipulate the output filename here as desired.
                        String fullZipToPath = Path.Combine(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ReceptorBus\BilbobusFTP\", entryFileName);
                        string directoryName = Path.GetDirectoryName(fullZipToPath);
                        if (directoryName.Length > 0)
                            Directory.CreateDirectory(directoryName);

                        using (FileStream streamWriter = File.Create(fullZipToPath))
                        {
                            StreamUtils.Copy(zipStream, streamWriter, buffer);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No se ha podido obtener las paradas de Euskotren. Compruebe su conexión a internet.");
                }
                
            }
            finally
            {
                if (zf != null)
                {
                    zf.IsStreamOwner = true; // Makes close also shut the underlying stream
                    zf.Close(); // Ensure we release resources
                }
            }
            if (zf != null)
            {
                Console.WriteLine("¡Completado!");

                Console.WriteLine("Leyendo rutas ...");

                int counter = 0;
                string line;

                // Leemos el fichero linea por linea para sacar las rutas
                System.IO.StreamReader file =
                   new System.IO.StreamReader(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ReceptorBus\BilbobusFTP\routes.txt");
                string primeraLinea = file.ReadLine();
                while ((line = file.ReadLine()) != null)
                {

                    //Obtengo la info que me interesa
                    string[] valoresLinea = line.Split(',');

                    //ID de ruta
                    string id = valoresLinea[0];

                    //ID de agencia
                    string idAgencia = valoresLinea[1];

                    //Nombre corto ruta
                    string abreviatura = valoresLinea[2];

                    //Nombre completo de la ruta
                    string nombre = valoresLinea[3];

                    //Tipo de ruta
                    int tipo = int.Parse(valoresLinea[5]);

                    //Añado a la lista
                    lineas_bilbobus l = new lineas_bilbobus();
                    l.idLinea = id;
                    l.abreviatura = abreviatura;
                    l.nombre = nombre;
                    l.tipoTransporte = tipo;

                    lineasBilbo.Add(id, l);

                }

                file.Close();

                Console.WriteLine("¡Completado rutas!");
                Console.WriteLine("Leyendo viajes...");

                // Leemos el fichero linea por linea para sacar los viajes
                Dictionary<int, viajes_bilbobus> viajes = new Dictionary<int, viajes_bilbobus>();
                file = new System.IO.StreamReader(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ReceptorBus\BilbobusFTP\stop_times.txt");
                primeraLinea = file.ReadLine();
                while ((line = file.ReadLine()) != null)
                {

                    //Obtengo la info que me interesa
                    string[] valoresLinea = line.Split(',');

                    //ID de viaje
                    int id = int.Parse(valoresLinea[0]);

                    //TiempoLlegada
                    string tiempoLlegada = valoresLinea[1];

                    //TiempoSalida
                    string tiempoSalida = valoresLinea[2];

                    //ID de la parada
                    int idParada = int.Parse(valoresLinea[3]);

                    //Comprobamos si contiene la clave
                    bool encontrado = false;
                    int index = 0;

                    viajes_parada_tiempos_bilbobus vpt = new viajes_parada_tiempos_bilbobus();
                    vpt.tiempoLlegada = tiempoLlegada;
                    vpt.tiempoSalida = tiempoSalida;
                    vpt.paradas_bilbobus = paradasBilbobus[idParada];

                    if (viajes.ContainsKey(id))
                    {
                        //Se añade solo los tiempos por esa parada al viaje
                        viajes[id].viajes_parada_tiempos_bilbobus.Add(vpt);
                        viajes[id].tiempoFin = tiempoLlegada;

                    }
                    else
                    {
                        //Se añade el viaje completo
                        viajes_bilbobus v = new viajes_bilbobus();
                        v.id = id;
                        v.tiempoInicio = tiempoLlegada;
                        v.tiempoFin = tiempoSalida;
                        v.viajes_parada_tiempos_bilbobus.Add(vpt);
                        viajes.Add(id, v);
                    }
                }

                file.Close();

                Console.WriteLine("¡Completado viajes!");
                Console.WriteLine("Juntando resultados...");

                // Leemos el fichero linea por linea
                file = new System.IO.StreamReader(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ReceptorBus\BilbobusFTP\trips.txt");
                primeraLinea = file.ReadLine();
                while ((line = file.ReadLine()) != null)
                {

                    //Obtengo la info que me interesa
                    string[] valoresLinea = line.Split(',');

                    //ID de ruta
                    string idRuta = valoresLinea[0];

                    //ID del viaje
                    int idViaje = int.Parse(valoresLinea[2]);

                    try
                    {
                        if (viajes.ContainsKey(idViaje))
                        {
                            lineasBilbo[idRuta].viajes_bilbobus.Add(viajes[idViaje]);
                        }
                        else
                        {
                            throw new KeyNotFoundException();
                        }
                    }
                    catch (System.Collections.Generic.KeyNotFoundException e)
                    {
                        Console.WriteLine("No se encuentra el viaje de id: " + idViaje + " perteneciente a la ruta " +
                                          idRuta + "\n");
                    }
                }

                file.Close();
                Console.WriteLine("¡Completada la unión de resultados!");
            }
            

        }

        private void lineasEuskotren()
        {
            if (paradasBizkaibus.Count != 0)
            {
                paradasBizkaibus.Clear();
            }

            Console.WriteLine("Empiezo Lineas Euskotren");
            if (
                !File.Exists(
                    @"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ReceptorBus\EuskotrenFTP\google_transit.zip"))
            {
                FTP cliente = new FTP("ftp://ftp.geo.euskadi.net/cartografia/Transporte/Moveuskadi/", "", "");
                cliente.download("Euskotren/google_transit.zip", @"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ReceptorBus\EuskotrenFTP\google_transit.zip");
            }
            

            Console.WriteLine("¡Completado!");

            //Procesar los archivos
            Console.WriteLine("Extranyendo...");

            ZipFile zf = null;
            try
            {
                //Establezco el fichero a descomprimir
                FileStream fs = File.OpenRead(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ReceptorBus\EuskotrenFTP\google_transit.zip");
                try
                {
                    zf = new ZipFile(fs);
                }
                catch (System.ArgumentException e)
                {
                    Console.WriteLine("El fichero está dañado o la descarga no se ha realizado correctamente.");
                    zf = null;
                }

                if (zf != null)
                {
                    //Por cada archivo dentro del zip, voy descomprimiendo
                    foreach (ZipEntry zipEntry in zf)
                    {
                        //Aunque no habrá directorios en nuestro caso, si los hubiera, los ignora
                        if (!zipEntry.IsFile)
                        {
                            continue;
                        }
                        //Obtiene el nombre del fichero que contiene el zip
                        String entryFileName = zipEntry.Name;


                        byte[] buffer = new byte[4096];
                        Stream zipStream = zf.GetInputStream(zipEntry);

                        // Manipulate the output filename here as desired.
                        String fullZipToPath = Path.Combine(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ReceptorBus\EuskotrenFTP\", entryFileName);
                        string directoryName = Path.GetDirectoryName(fullZipToPath);
                        if (directoryName.Length > 0)
                            Directory.CreateDirectory(directoryName);

                        using (FileStream streamWriter = File.Create(fullZipToPath))
                        {
                            StreamUtils.Copy(zipStream, streamWriter, buffer);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No se ha podido obtener las paradas de Euskotren. Compruebe su conexión a internet.");
                }

            }
            finally
            {
                if (zf != null)
                {
                    zf.IsStreamOwner = true; // Makes close also shut the underlying stream
                    zf.Close(); // Ensure we release resources
                }
            }
            if (zf != null)
            {
                Console.WriteLine("¡Completado!");

                Console.WriteLine("Leyendo rutas ...");

                int counter = 0;
                string line;

                // Leemos el fichero linea por linea para sacar las rutas
                System.IO.StreamReader file =
                   new System.IO.StreamReader(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ReceptorBus\EuskotrenFTP\routes.txt");
                string primeraLinea = file.ReadLine();
                while ((line = file.ReadLine()) != null)
                {

                    //Obtengo la info que me interesa
                    string[] valoresLinea = line.Split(',');

                    //ID de ruta
                    int id = int.Parse(valoresLinea[0]);

                    //ID de agencia
                    string idAgencia = valoresLinea[1];

                    //Nombre corto ruta
                    string abreviatura = valoresLinea[2];

                    //Nombre completo de la ruta
                    string nombre = valoresLinea[3];

                    //Tipo de ruta
                    int tipo = int.Parse(valoresLinea[5]);

                    //Añado a la lista
                    lineas_euskotren l = new lineas_euskotren();
                    l.id = id;
                    l.abreviatura = abreviatura;
                    l.nombre = nombre;
                    l.tipo = tipo;
                    lineasEusko.Add(id, l);

                }

                file.Close();

                Console.WriteLine("¡Completado rutas!");
                Console.WriteLine("Leyendo viajes...");

                // Leemos el fichero linea por linea para sacar los viajes
                Dictionary<string, viajes_euskotren> viajes = new Dictionary<string, viajes_euskotren>();
                file = new System.IO.StreamReader(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ReceptorBus\EuskotrenFTP\stop_times.txt");
                primeraLinea = file.ReadLine();
                while ((line = file.ReadLine()) != null)
                {

                    //Obtengo la info que me interesa
                    string[] valoresLinea = line.Split(',');

                    //ID de viaje
                    string id = valoresLinea[0];

                    //TiempoLlegada
                    string tiempoLlegada = valoresLinea[1];

                    //TiempoSalida
                    string tiempoSalida = valoresLinea[2];

                    //ID de la parada
                    int idParada = int.Parse(valoresLinea[3]);

                    //Comprobamos si contiene la clave
                    bool encontrado = false;
                    int index = 0;

                    viajes_parada_tiempos_euskotren vpt = new viajes_parada_tiempos_euskotren();
                    vpt.tiempoLlegada = tiempoLlegada;
                    vpt.tiempoSalida = tiempoSalida;
                    vpt.paradas_euskotren = paradasEuskotren[idParada];

                    if (viajes.ContainsKey(id))
                    {
                        //Se añade solo los tiempos por esa parada al viaje
                        viajes[id].viajes_parada_tiempos.Add(vpt);
                        viajes[id].tiempoFin = tiempoLlegada;

                    }
                    else
                    {
                        //Se añade el viaje completo
                        viajes_euskotren v = new viajes_euskotren();
                        v.idViaje = id;
                        v.tiempoInicio = tiempoLlegada;
                        v.tiempoFin = tiempoSalida;
                        v.viajes_parada_tiempos.Add(vpt);
                        viajes.Add(id, v);
                    }

                /*    if (viajes.ContainsKey(id))
                    {
                        //Se añade solo al viaje la parada
                        try
                        {
                            viajes[id].paradas.Add(idParada, paradasEuskotren[idParada]);
                        }
                        catch(System.ArgumentException ex)
                        {
                            Console.WriteLine("Parada " + idParada + "repetida en "+ id+ " viaje");
                        }
                        

                    }
                    else
                    {
                        //Se añade el viaje completo
                        viajes.Add(id, new ViajeEuskotren(id, paradasEuskotren[idParada]));
                    }*/
                }

                file.Close();

                Console.WriteLine("¡Completado viajes!");
                Console.WriteLine("Juntando resultados...");

                // Leemos el fichero linea por linea
                file = new System.IO.StreamReader(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ReceptorBus\EuskotrenFTP\trips.txt");
                primeraLinea = file.ReadLine();
                while ((line = file.ReadLine()) != null)
                {

                    //Obtengo la info que me interesa
                    string[] valoresLinea = line.Split(',');

                    //ID de ruta
                    int idRuta = int.Parse(valoresLinea[0]);

                    //ID del viaje
                    string idViaje = valoresLinea[2];

                    try
                    {
                        if (viajes.ContainsKey(idViaje))
                        {
                            lineasEusko[idRuta].viajes_euskotren.Add(viajes[idViaje]);
                        }
                        else
                        {
                            throw new KeyNotFoundException();
                        }
                    }
                    catch (System.Collections.Generic.KeyNotFoundException e)
                    {
                        Console.WriteLine("No se encuentra el viaje de id: " + idViaje + " perteneciente a la ruta " + idRuta + "\n");
                    }
                }
                Console.WriteLine("¡FIN de Juntando resultados!");
                file.Close();


            }


        }

        private void lineasMetro()
        {
            if (lineasMetroBilbao.Count != 0)
            {
                lineasMetroBilbao.Clear();
            }

            Console.WriteLine("Empiezo Lineas Metro");
            if (
                !File.Exists(
                    @"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ReceptorBus\MetroFTP\google_transit.zip"))
            {
                FTP cliente = new FTP("ftp://ftp.geo.euskadi.net/cartografia/Transporte/Moveuskadi/", "", "");
                cliente.download("MetroBilbao/google_transit.zip",
                    @"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ReceptorBus\MetroFTP\google_transit.zip");
            }

            Console.WriteLine("¡Completado!");

            //Procesar los archivos
            Console.WriteLine("Extranyendo...");

            ZipFile zf = null;
            try
            {
                //Establezco el fichero a descomprimir
                FileStream fs = File.OpenRead(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ReceptorBus\MetroFTP\google_transit.zip");
                try
                {
                    zf = new ZipFile(fs);
                }
                catch (System.ArgumentException e)
                {
                    Console.WriteLine("El fichero está dañado o la descarga no se ha realizado correctamente.");
                    zf = null;
                }

                if (zf != null)
                {
                    //Por cada archivo dentro del zip, voy descomprimiendo
                    foreach (ZipEntry zipEntry in zf)
                    {
                        //Aunque no habrá directorios en nuestro caso, si los hubiera, los ignora
                        if (!zipEntry.IsFile)
                        {
                            continue;
                        }
                        //Obtiene el nombre del fichero que contiene el zip
                        String entryFileName = zipEntry.Name;


                        byte[] buffer = new byte[4096];
                        Stream zipStream = zf.GetInputStream(zipEntry);

                        // Manipulate the output filename here as desired.
                        String fullZipToPath = Path.Combine(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ReceptorBus\MetroFTP\", entryFileName);
                        string directoryName = Path.GetDirectoryName(fullZipToPath);
                        if (directoryName.Length > 0)
                            Directory.CreateDirectory(directoryName);

                        using (FileStream streamWriter = File.Create(fullZipToPath))
                        {
                            StreamUtils.Copy(zipStream, streamWriter, buffer);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No se ha podido obtener las paradas de Euskotren. Compruebe su conexión a internet.");
                }

            }
            finally
            {
                if (zf != null)
                {
                    zf.IsStreamOwner = true; // Makes close also shut the underlying stream
                    zf.Close(); // Ensure we release resources
                }
            }
            if (zf != null)
            {
                Console.WriteLine("¡Completado!");

                Console.WriteLine("Leyendo rutas ...");

                int counter = 0;
                string line;

                // Leemos el fichero linea por linea para sacar las rutas
                System.IO.StreamReader file =
                   new System.IO.StreamReader(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ReceptorBus\MetroFTP\routes.txt");
                string primeraLinea = file.ReadLine();
                while ((line = file.ReadLine()) != null)
                {

                    //Obtengo la info que me interesa
                    string[] valoresLinea = line.Split(',');

                    //ID de ruta
                    string id = valoresLinea[0];

                    //Nombre corto ruta
                    string abreviatura = valoresLinea[1];

                    //Nombre completo de la ruta
                    string nombre = valoresLinea[2];

                    //Tipo de ruta
                    int tipo = int.Parse(valoresLinea[3]);

                    //Añado a la lista
                    lineas_metro l = new lineas_metro();
                    l.idMetro = id;
                    l.abreviatura = abreviatura;
                    l.nombre = nombre;
                    l.tipo = tipo;
                    lineasMetroBilbao.Add(id, l);
                }

                file.Close();

                Console.WriteLine("¡Completado rutas!");
                Console.WriteLine("Leyendo viajes...");

                // Leemos el fichero linea por linea para sacar los viajes
                SortedDictionary<int, viajes_metro> viajes = new SortedDictionary<int, viajes_metro>();
                file = new System.IO.StreamReader(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ReceptorBus\MetroFTP\stop_times.txt");
                primeraLinea = file.ReadLine();
                while ((line = file.ReadLine()) != null)
                {

                    //Obtengo la info que me interesa
                    string[] valoresLinea = line.Split(',');

                    //ID de viaje
                    int id = int.Parse(valoresLinea[0]);

                    //Secuencia de parada
                    int secuencia = int.Parse(valoresLinea[1]);

                    //tiempoLlegada
                    string tiempoLlegada = valoresLinea[2];

                    //tiempoSalida
                    string tiempoSalida = valoresLinea[3];

                    //ID de la parada
                    string idParada = valoresLinea[4];

                    //****
                    viajes_parada_tiempos_metro vpt = new viajes_parada_tiempos_metro();
                    vpt.tiempoLlegada = tiempoLlegada;
                    vpt.tiempoSalida = tiempoSalida;
                    vpt.paradas_metro = paradasMetro[idParada];

                    if (viajes.ContainsKey(id))
                    {
                        //Se añade solo los tiempos por esa parada al viaje
                        viajes[id].viajes_parada_tiempos_metro.Add(vpt);
                        viajes[id].tiempoFin = tiempoLlegada;

                    }
                    else
                    {
                        //Se añade el viaje completo
                        viajes_metro v = new viajes_metro();
                        v.id= id;
                        v.tiempoInicio = tiempoLlegada;
                        v.tiempoFin = tiempoSalida;
                        v.viajes_parada_tiempos_metro.Add(vpt);
                        viajes.Add(id, v);
                    }
                }
                file.Close();

                Console.WriteLine("¡Completado viajes!");
                Console.WriteLine("Juntando resultados...");

                // Leemos el fichero linea por linea
                file = new System.IO.StreamReader(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ReceptorBus\MetroFTP\trips.txt");
                primeraLinea = file.ReadLine();
                while ((line = file.ReadLine()) != null)
                {

                    //Obtengo la info que me interesa
                    string[] valoresLinea = line.Split(',');

                    //ID de ruta
                    string idRuta = valoresLinea[1];

                    //ID del viaje
                    int idViaje = int.Parse(valoresLinea[0]);

                    try
                    {
                        if (viajes.ContainsKey(idViaje))
                        {
                            lineasMetroBilbao[idRuta].viajes_metro.Add(viajes[idViaje]);
                        }
                        else
                        {
                            throw new KeyNotFoundException();
                        }
                    }
                    catch (System.Collections.Generic.KeyNotFoundException e)
                    {
                        Console.WriteLine("No se encuentra el viaje de id: " + idViaje + " perteneciente a la ruta " + idRuta + "\n");
                    }
                }
                Console.WriteLine("¡FIN de Juntando resultados!");

                file.Close();

            }


        }
        static int Compare1(Clases.KeyValuePair<string, int> a, Clases.KeyValuePair<string, int> b)
        {
            return a.Key.CompareTo(b.Key);
        }

        private void lineasBizkaibus()
        {
            Console.WriteLine("Empiezo Lineas Euskotren");
            if (
                !File.Exists(
                    @"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ReceptorBus\BizkaibusFTP\google_transit.zip"))
            {
                FTP cliente = new FTP("ftp://ftp.geo.euskadi.net/cartografia/Transporte/Moveuskadi/", "", "");
                cliente.download("Bizkaibus/google_transit.zip", @"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ReceptorBus\BizkaibusFTP\google_transit.zip");
            }


            Console.WriteLine("¡Completado!");

            //Procesar los archivos
            Console.WriteLine("Extranyendo...");

            ZipFile zf = null;
            try
            {
                //Establezco el fichero a descomprimir
                FileStream fs = File.OpenRead(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ReceptorBus\BizkaibusFTP\google_transit.zip");
                try
                {
                    zf = new ZipFile(fs);
                }
                catch (System.ArgumentException e)
                {
                    Console.WriteLine("El fichero está dañado o la descarga no se ha realizado correctamente.");
                    zf = null;
                }

                if (zf != null)
                {
                    //Por cada archivo dentro del zip, voy descomprimiendo
                    foreach (ZipEntry zipEntry in zf)
                    {
                        //Aunque no habrá directorios en nuestro caso, si los hubiera, los ignora
                        if (!zipEntry.IsFile)
                        {
                            continue;
                        }
                        //Obtiene el nombre del fichero que contiene el zip
                        String entryFileName = zipEntry.Name;


                        byte[] buffer = new byte[4096];
                        Stream zipStream = zf.GetInputStream(zipEntry);

                        // Manipulate the output filename here as desired.
                        String fullZipToPath = Path.Combine(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ReceptorBus\BizkaibusFTP\", entryFileName);
                        string directoryName = Path.GetDirectoryName(fullZipToPath);
                        if (directoryName.Length > 0)
                            Directory.CreateDirectory(directoryName);

                        using (FileStream streamWriter = File.Create(fullZipToPath))
                        {
                            StreamUtils.Copy(zipStream, streamWriter, buffer);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No se ha podido obtener las paradas de Euskotren. Compruebe su conexión a internet.");
                }

            }
            finally
            {
                if (zf != null)
                {
                    zf.IsStreamOwner = true; // Makes close also shut the underlying stream
                    zf.Close(); // Ensure we release resources
                }
            }
            if (zf != null)
            {
                Console.WriteLine("¡Completado!");

                Console.WriteLine("Leyendo rutas ...");

                int counter = 0;
                string line;

                // Leemos el fichero linea por linea para sacar las rutas
                System.IO.StreamReader file =
                   new System.IO.StreamReader(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ReceptorBus\BizkaibusFTP\routes.txt");
                string primeraLinea = file.ReadLine();
                while ((line = file.ReadLine()) != null)
                {

                    //Obtengo la info que me interesa
                    string[] valoresLinea = line.Split(',');

                    //ID de ruta
                    int id = int.Parse(valoresLinea[0]);

                    //ID de agencia
                    string idAgencia = valoresLinea[1];

                    //Nombre corto ruta
                    string abreviatura = valoresLinea[2];

                    //Nombre completo de la ruta
                    string nombre = valoresLinea[3];

                    //Tipo de ruta
                    int tipo = int.Parse(valoresLinea[5]);

                    //Añado a la lista
                    lineas_bizkaibus lb = new lineas_bizkaibus();
                    lb.id = id;
                    lb.abreviatura = abreviatura;
                    lb.nombre = nombre;
                    lb.tipoTransporte = tipo;
                    lineasBizkaia.Add(id, lb);

                }

                file.Close();

                Console.WriteLine("¡Completado rutas!");
                Console.WriteLine("Leyendo viajes...");

                // Leemos el fichero linea por linea para sacar los viajes
                Dictionary<int, viajes_bizkaibus> viajes = new Dictionary<int, viajes_bizkaibus>();
                file = new System.IO.StreamReader(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ReceptorBus\BizkaibusFTP\stop_times.txt");
                primeraLinea = file.ReadLine();
                while ((line = file.ReadLine()) != null)
                {

                    //Obtengo la info que me interesa
                    string[] valoresLinea = line.Split(',');

                    //ID de viaje
                    int id = int.Parse(valoresLinea[0]);

                    //TiempoLlegada
                    string tiempoLlegada = valoresLinea[1];

                    //TiempoSalida
                    string tiempoSalida = valoresLinea[2];

                    //ID de la parada
                    int idParada = int.Parse(valoresLinea[3]);

                    //Se añade solo al viaje la parada
                    try
                    {
                        viajes_parada_tiempos_bizkaibus vpt = new viajes_parada_tiempos_bizkaibus();
                        vpt.tiempoLlegada = tiempoLlegada;
                        vpt.tiempoSalida = tiempoSalida;
                        vpt.paradas_bizkaibus = paradasBizkaibus[idParada];

                        if (viajes.ContainsKey(id))
                        {
                            //Se añade solo los tiempos por esa parada al viaje
                            viajes[id].viajes_parada_tiempos_bizkaibus.Add(vpt);
                            viajes[id].tiempoFin = tiempoLlegada;

                        }
                        else
                        {
                            //Se añade el viaje completo
                            viajes_bizkaibus v = new viajes_bizkaibus();
                            v.id = id;
                            v.tiempoInicio = tiempoLlegada;
                            v.tiempoFin = tiempoSalida;
                            v.viajes_parada_tiempos_bizkaibus.Add(vpt);
                            viajes.Add(id, v);
                        }


                    }
                    catch (System.ArgumentException ex)
                    {
                        Console.WriteLine("Parada " + idParada + "repetida en " + id + " viaje");
                    }
                }

                file.Close();

                Console.WriteLine("¡Completado viajes!");
                Console.WriteLine("Juntando resultados...");

                // Leemos el fichero linea por linea
                file = new System.IO.StreamReader(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ReceptorBus\BizkaibusFTP\trips.txt");
                primeraLinea = file.ReadLine();
                while ((line = file.ReadLine()) != null)
                {

                    //Obtengo la info que me interesa
                    string[] valoresLinea = line.Split(',');

                    //ID de ruta
                    int idRuta = int.Parse(valoresLinea[0]);

                    //ID del viaje
                    int idViaje = int.Parse(valoresLinea[2]);

                    try
                    {
                            if (viajes.ContainsKey(idViaje))
                            {
                                lineasBizkaia[idRuta].viajes_bizkaibus.Add(viajes[idViaje]);
                            }
                            else
                            {
                                throw new KeyNotFoundException();
                            }
                        
                    }
                    catch (System.Collections.Generic.KeyNotFoundException e)
                    {
                        Console.WriteLine("No se encuentra el viaje de id: " + idViaje + " perteneciente a la ruta " + idRuta + "\n");
                    }
                }

                file.Close();


            }


        }

        private void tranvia()
        {
            XmlDocument tranvia = descargaDeURL("https://mapsengine.google.com/map/kml?mid=z5Ze7dtSm54M.krTEjXdoqZCo&forcekml=1");
            if (tranvia != null)
            {
                Console.WriteLine(tranvia.ChildNodes[0].Name);
                XmlNodeList temp = tranvia.SelectNodes("//*");

                Console.WriteLine("Empiezo Foreach");
                XmlNode paradas = temp[0].ChildNodes[0].ChildNodes[2];
                bool primero = true;
                foreach (XmlNode parada in paradas.ChildNodes)
                {
                    if (primero)
                    {
                        primero = false;
                    }
                    else
                    {
                        //Aqui parseo las paradas que voy a utilizar
                        if (!parada.ChildNodes[0].InnerText.Equals("Tranvía Bilbao"))
                        {
                            if (!parada.ChildNodes[0].InnerText.Equals("Marca de posición 13"))
                            {
                                //Console.WriteLine(parada.ChildNodes[0].InnerText);
                                string nombre = parada.ChildNodes[0].InnerText;
                                if (nombre.Equals("Arriga"))
                                {
                                    nombre = "Arriaga";
                                }
                                string coordenadastxt;
                                string descripcion;
                                if(parada.ChildNodes[1].Name.Equals("description"))
                                {
                                    coordenadastxt = parada.ChildNodes[4].InnerText;
                                    descripcion = parada.ChildNodes[1].InnerText;
                                }
                                else
                                {
                                    coordenadastxt = parada.ChildNodes[3].InnerText;
                                    descripcion = "Desconocida";
                                }
                                    
                                string[] coordenadas = coordenadastxt.Split(',');
                                double latitud = double.Parse(coordenadas[1], CultureInfo.InvariantCulture);
                                double longitud = double.Parse(coordenadas[0], CultureInfo.InvariantCulture);
                                
                                paradas_tranvia p = new paradas_tranvia();
                                p.nombre = nombre;
                                p.latitud = latitud;
                                p.longitud = longitud;
                                p.descripcion = descripcion;

                                paradasTranvia.Add(p);

                            }
                            
                        }
                    }
                    
                }
            }
        }

    }
}
