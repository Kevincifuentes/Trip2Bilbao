using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Almacenamiento;
using Clases;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using WrappersActiveMQ;

namespace ExtractorDatos
{
    class InformacionEstatica
    {
        

        public InformacionEstatica()
        {
            emisor.inicializar();
            //contadorDBSDeusto = 0;
            //contadorGeneralDeusto = 0;
        }
 
        public Dictionary<int, Parking> parkings = new Dictionary<int, Parking>();
        public Dictionary<int, ParadaBilbo> paradasBilbobus = new Dictionary<int, ParadaBilbo>();
        private List<ParadaTranvia> paradasTranvia = new List<ParadaTranvia>(); 
        private Dictionary<int, ParadaBizkaibus> paradasBizkaibus= new Dictionary<int, ParadaBizkaibus>();
        private Dictionary<string , ParadaMetro> paradasMetro = new Dictionary<string, ParadaMetro>(); 
        private Dictionary<int, ParadaEuskotren> paradasEuskotren = new Dictionary<int, ParadaEuskotren>();
        public Dictionary<string, LineaBilbobus> lineasBilbo = new Dictionary<string, LineaBilbobus>();
        private Dictionary<int, LineaEuskotren> lineasEusko = new Dictionary<int, LineaEuskotren>();
        private Dictionary<int, LineaBizkaibus> lineasBizkaia = new Dictionary<int, LineaBizkaibus>();
        private Dictionary<string, LineaMetro> lineasMetroBilbao = new Dictionary<string, LineaMetro>();  
        private List<Farmacia> farmaciasList = new List<Farmacia>(); 
        private List<Hospital> hospitalList = new List<Hospital>();
        private List<CentroSalud> centroSaludList = new List<CentroSalud>();
        //private List<PuntoBici> puntosBicisList = new List<PuntoBici>();
        //private int contadorGeneralDeusto;
        //private int contadorDBSDeusto;
        public static EmisorBus emisor = new EmisorBus();

        public DateTime descargaParkings;

        /*static void Main(string[] args)
        {
            InformacionEstatica p = new InformacionEstatica();
            Console.WriteLine("-- Tipo de datos --");
            Console.WriteLine("1. Información Estática");
            Console.WriteLine("2. Información Dinámica");
            char n = char.Parse((Console.ReadLine()));
            switch (n)
            {
                case '1':
                    Console.WriteLine("Estática");
                    p.informacionEstatica();
                    break;
                case '2':
                    Console.WriteLine("Dinámica");
                    InformacionDinamica inf = new InformacionDinamica(p);
                    // Por rellenar
                    break;
            }



            //Información dinámica de un recinto de aparcamiento (Necesita id)
           // p.descargaDeURL("http://www.geobilbao.net/wfsCocities?service=wfs&version=1.1.0&request=GetFeature&typeName=eti:CarParkDynamic");


        }*/

        public void informacionEstatica()
        {
            InformacionEstatica p = this;
            int c;
            do
            {
                Console.WriteLine("1. Información sobre los recintos de aparcamiento.");
                Console.WriteLine("2. Paradas de autobuses");
                Console.WriteLine("3. Farmacias");
                Console.WriteLine("4. Hospitales");
                Console.WriteLine("5. Centros de salud");
                Console.WriteLine("6. Bicicletas");
                Console.WriteLine("7. Parking de Deusto");
                Console.WriteLine("8. Paradas Metro");
                Console.WriteLine("9. Paradas Euskotren");
                Console.WriteLine("10. Tranvía Bilbao");
                Console.WriteLine("11. SALIR");

                c = int.Parse(Console.ReadLine());
                switch (c)
                {
                    case 1:
                        Console.WriteLine("Case 1");
                        int bien = 0;
                        int mal = 0;
                        p.recintosAparcamiento();
                        p.recintosAparcamientoEstatico();
                        Console.WriteLine("Parkings: Bien "+ bien+ " Mal "+mal);
                        break;
                    case 2:
                        Console.WriteLine("Case 2");
                        Console.WriteLine("1. Bilbobus");
                        Console.WriteLine("2. Bizkaibus");
                        char c2 = char.Parse(Console.ReadLine());
                        switch (c2)
                        {
                            case '1':
                                Console.WriteLine("Case 1");
                                p.paradasAutobusesBilbo();
                                Console.WriteLine(lineasBilbo.Count);
                                InformacionDinamica i = new InformacionDinamica(p);
                                Console.WriteLine(lineasBilbo.Count);
                                p.lineasBilbobus();
                                Console.WriteLine(lineasBilbo.Count);
                                Console.ReadLine();
                                //emisor.enviarTiemposParadas(paradasBilbobus);
                                //emisor.enviarTiemposLineas(lineasBilbo);
                                
                                break;
                            case '2':
                                Console.WriteLine("Case 2");
                                p.paradasAutobusesBizkaia();
                                emisor.enviarParadasBizkaibus(paradasBizkaibus);
                                p.lineasBizkaibus();
                                emisor.enviarLineasBizkaibus(lineasBizkaia);
                                break;
                        }
                        break;
                    case 3:
                        Console.WriteLine("Case 3");
                        p.farmacias();
                        emisor.enviarFarmacias(farmaciasList);
                        /*foreach (Farmacia f in p.farmaciasList)
                    {
                        Console.WriteLine(f.ToString());
                    }*/
                        Console.ReadKey();
                        break;
                    case 4:
                        Console.WriteLine("Case 4");
                        p.hospitales();
                        Insertar insertar = new Insertar();
                        insertar.hospital(hospitalList[0]);
                        break;
                    case 5:
                        Console.WriteLine("Case 5");
                        p.centrosDeSalud();
                        //emisor.enviarCentrosSalud(centroSaludList);
                        //Insertar insertar = new Insertar();
                        //insertar.centrodeSalud(centroSaludList[0]);
                        break;
                    case 6:
                        Console.WriteLine("Case 6");
                        //p.bicicletas();
                        //emisor.enviarBicicletas(puntosBicisList);
                       /* foreach (PuntoBici bici in p.puntosBicisList)
                        {
                            Console.WriteLine(bici.ToString());
                        }*/
                        break;
                    case 7:
                        Console.WriteLine("Case 7");
                        //p.parkingDeusto();
                        //emisor.enviarParkingDeusto(contadorDBSDeusto, contadorGeneralDeusto);
                        break;
                    case 8:
                        Console.WriteLine("Case 8");
                        metroBilbao();
                        p.lineasMetro();
                        emisor.enviarLineasMetro(lineasMetroBilbao);
                        break;
                    case 9:
                        Console.WriteLine("Case 9");
                        euskotren();
                        emisor.enviarParadasEuskotren(paradasEuskotren);
                        p.lineasEuskotren();
                        emisor.enviarLineasEuskotren(lineasEusko);
                        break;
                    case 10:
                        Console.WriteLine("Case 10");
                        tranvia();
                        break;
                    default:
                        Console.WriteLine("Default case");
                        break;
                }
                
            } while (c!=11);
        }



        private XmlDocument descargaDeURL(string url)
        {

            XmlDocument document = new XmlDocument();
            try
            {
                document.Load(url);
            }
            catch(System.Net.WebException e)
            {
                Console.WriteLine("Error al intentar obtener la url (WebException): " + url);
                document = null;
            }
            catch (System.Xml.XmlException ex)
            {
                Console.WriteLine("Error al intentar obtener la url (XmlException): " + url);
                Console.Write(ex.Message);
              /*  document.Load(new StreamReader(url, Encoding.GetEncoding("ISO-8859-9")));
                Console.WriteLine(document.InnerText);*/
                document = null;
                
            }
            return document;
        }

        //Obtengo la información estática mediante lo que he obtenido dinámicamente

        public void obtenerInformacionParkings()
        {
            descargaParkings = DateTime.Now;
            recintosAparcamiento();
            recintosAparcamientoEstatico();    
        }

        private void recintosAparcamiento()
        {
            if (parkings.Count != 0)
            {
                parkings.Clear();
            }

            Console.WriteLine("Empiezo Aparcamiento");
            XmlDocument aparcamientosxml=this.descargaDeURL("http://www.geobilbao.net/wfsCocities?service=wfs&version=1.1.0&request=GetFeature&typeName=eti:CarParkDynamic"); 
        
            //Los Namespaces no funcionan (DESACTUALIZADOS)
            /*XmlNamespaceManager nsm = new XmlNamespaceManager(aparcamientosxml.NameTable);
            nsm.AddNamespace("wfs", "http://www.geobilbao.net/wfsCocities/schemas/wfs/1.1.0/wfs.xsd");
            nsm.AddNamespace("gml", "http://schemas.opengis.net/gml/3.1.1/base/gml.xsd");*/

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
                    Parking temporal = new Parking();
                    temporal.id = int.Parse(id);
                    Console.WriteLine("Despues del parse: " + temporal.id);


                    //Empezamos a obtener los datos para ese ID especifico
                    // Nodo por Nodo
                    int indice = 0;
                    Console.WriteLine(node.ChildNodes[indice].FirstChild);

                    //Ocupacion
                    if (node.ChildNodes[indice].Name.Equals("eti:occupancy"))
                    {
                        double ocupacion = double.Parse(node.ChildNodes[indice].InnerText, CultureInfo.InvariantCulture);
                        temporal.ocupacion = ocupacion;
                        Console.WriteLine("Despues del parse ocupacion: " + temporal.ocupacion);
                        indice++;
                    }
                    else
                    {
                        //No hay información
                        temporal.ocupacion = -1.0;
                    }

                    //Porcentaje
                    if (node.ChildNodes[indice].Name.Equals("eti:occupancyPercentage"))
                    {
                        Console.WriteLine(node.ChildNodes[indice].InnerText);
                        double porcentaje = double.Parse(node.ChildNodes[indice].InnerText, CultureInfo.InvariantCulture);
                        temporal.porcentaje = porcentaje;
                        Console.WriteLine("Despues del parse ocupacion: " + temporal.porcentaje);
                        indice++;
                    }
                    else
                    {
                        //No hay información
                        temporal.porcentaje = -1.0;
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
                    parkings.Add(temporal.id, temporal);


                }
                Console.WriteLine("Acabo Foreach");
                foreach (Parking p2 in parkings.Values)
                {
                    //Para obtener solo la información actualiazada
                    if (p2.fecha.Day == DateTime.Today.Day)
                    { Console.WriteLine(p2.ToString()); }

                }
                Console.WriteLine("Número total de parkings: " + parkings.Count);
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
                    if(node.Name.Equals("ows:ExceptionText"))
                    {
                        Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>ERROR EN EL SERVIDOR DE DESCARGA DE OPENDATA<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
                    }
                    else
                    {
                        //Variables a dar valor
                        string nombreParking = "Desconocido";
                        Coordenadas c = new Coordenadas();
                        List<Entrada> entradas = new List<Entrada>();
                        int capacidad = 0;
                        string tipo = "Desconocido";
                        List<Tarifa> tarifas = new List<Tarifa>();

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

                            Entrada entrada = new Entrada();

                            entrada.nombre = "Entrada " + numeroEntrada;
                            Console.WriteLine(entrada.nombre);
                            entrada.puntoEntrada = new Coordenadas(latitud, longitud);
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
                        if(capacidadS.Equals("underground"))
                        {
                            tipo = "Subterraneo";
                        }
                        else
                        {
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

                                Tarifa t = new Tarifa(dias, descripción, ne);
                                tarifas.Add(t);
                                indice++;
                            }
                        }
                       

                        // Añadimos al parking en cuestion
                        /*string nombreParking = "Desconocido";
                        Coordenadas c = new Coordenadas();
                        LinkedList<Entrada> entradas = new LinkedList<Entrada>();
                        int capacidad = 0;
                        string tipo = "Desconocido";*/
                        Parking p = parkings[id];
                        if (p != null)
                        {
                            Console.WriteLine("Se ha encontrado. ID= " + p.id);
                            p.latlong = c;
                            p.nombre = nombreParking;
                            p.entradas = entradas;
                            p.capacidad = capacidad;
                            p.tipo = tipo;
                            p.tarifas = tarifas;
                        }
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

                    ParadaBilbo temporal = new ParadaBilbo(id, idParada, new Coordenadas(latitud, longitud), nombreCompleto, nombreAbreviado);
                    paradasBilbobus.Add(idParada, temporal);

                }
            }
           

        }

        private void paradasAutobusesBizkaia()
        {
            if (paradasBizkaibus.Count != 0)
            {
                paradasBizkaibus.Clear();
            }

            Console.WriteLine("Empiezo descarga Bizkaibus");
            FTP cliente = new FTP("ftp://ftp.geo.euskadi.net/cartografia/Transporte/Moveuskadi/", "", "");
            cliente.download("Bizkaibus/google_transit.zip", @"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ExtractorDatos\BizkaibusFTP\google_transit.zip");
            Console.WriteLine("¡Completado!");

            //Procesar los archivos
            Console.WriteLine("Extranyendo...");

            ZipFile zf = null;
            try
            {
                //Establezco el fichero a descomprimir
                FileStream fs = File.OpenRead(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ExtractorDatos\BizkaibusFTP\google_transit.zip");
                try
                {
                    //Añado el stream a la utilidad ZipFile
                    zf = new ZipFile(fs);
                }
                catch (System.ArgumentException e)
                {
                    Console.WriteLine("El fichero está dañado o la descarga no se ha realizado correctamente.");
                }
                catch(ICSharpCode.SharpZipLib.Zip.ZipException ex)
                {
                    Console.WriteLine("No se ha podido obtener las paradas de Bizkaibus del zip. Compruebe su conexión a internet.");
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

                        //obtiene la ruta completa para el archivo nuevo a crear
                        String fullZipToPath = Path.Combine(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ExtractorDatos\BizkaibusFTP\", entryFileName);
                        string directoryName = Path.GetDirectoryName(fullZipToPath);

                        //se crea el directorio que se ha establecido
                        if (directoryName.Length > 0)
                            Directory.CreateDirectory(directoryName);

                        //se copia del stream del zip al stream de un nuevo archivo en la ruta especificada
                        using (FileStream streamWriter = File.Create(fullZipToPath))
                        {
                            StreamUtils.Copy(zipStream, streamWriter, buffer);
                        }
                        zipStream.Close();
                    }
                }
                
            }
            finally
            {
                if (zf != null)
                {
                    zf.IsStreamOwner = true; // Cierra el stream
                    zf.Close(); // Liberamos
                }
            }
            Console.WriteLine("¡Completado!");

            if (zf != null)
            {
                

                Console.WriteLine("Leyendo ficheros...");

                int counter = 0;
                string line;

                // Leemos el fichero linea por linea
                System.IO.StreamReader file =
                   new System.IO.StreamReader(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ExtractorDatos\BizkaibusFTP\stops.txt");
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
                    paradasBizkaibus.Add(id, new ParadaBizkaibus(id, codigo, nombreParada, descripcion, c, urlParada, tipoLocalizacion, idParadaPadre));
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
                    if (!node.ChildNodes[5].InnerText.Equals(""))
                    {
                        codigoPostal = int.Parse(node.ChildNodes[5].InnerText);
                    }

                    //Provincia
                    string provincia = node.ChildNodes[6].InnerText;

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
                    string ciudad = node.ChildNodes[10].InnerText;

                    //Url con más información (puede ser NULL)
                    string urlInfo = node.ChildNodes[11].InnerText;

                    //Ahora pasaremos a obtener más información de la Farmacia, pero desde otro XML enlazado a este
                    XmlDocument masInfo = this.descargaDeURL(node.ChildNodes[13].InnerText);
                    Coordenadas c = null;
                    long telefono = 00000000;
                    long fax = 000000000;
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
                            double latitud = double.Parse(masInfo.ChildNodes[1].ChildNodes[4].ChildNodes[6].ChildNodes[0].InnerText, CultureInfo.InvariantCulture);
                            double longitud = double.Parse(masInfo.ChildNodes[1].ChildNodes[4].ChildNodes[6].ChildNodes[1].InnerText, CultureInfo.InvariantCulture);
                            c = new Coordenadas(latitud, longitud);
                            //Información de contacto
                            //Telefono (si tiene)

                            if (!masInfo.ChildNodes[1].ChildNodes[5].ChildNodes[0].InnerText.Equals(""))
                            {
                                telefono = long.Parse(masInfo.ChildNodes[1].ChildNodes[5].ChildNodes[0].InnerText);
                            }

                            //Fax (si tiene)

                            if (!masInfo.ChildNodes[1].ChildNodes[5].ChildNodes[1].InnerText.Equals(""))
                            {
                                fax = long.Parse(masInfo.ChildNodes[1].ChildNodes[5].ChildNodes[1].InnerText);
                            }

                            //Email (si tiene)

                            if (!masInfo.ChildNodes[1].ChildNodes[5].ChildNodes[2].InnerText.Equals(""))
                            {
                                email = masInfo.ChildNodes[1].ChildNodes[5].ChildNodes[2].InnerText;
                            }

                            //Web (si tiene)

                            if (!masInfo.ChildNodes[1].ChildNodes[5].ChildNodes[3].InnerText.Equals(""))
                            {
                                web = masInfo.ChildNodes[1].ChildNodes[5].ChildNodes[3].InnerText;
                            }

                            //Info Adicional (si tiene)

                            if (!masInfo.ChildNodes[1].ChildNodes[5].ChildNodes[4].InnerText.Equals(""))
                            {
                                adicional = masInfo.ChildNodes[1].ChildNodes[5].ChildNodes[4].InnerText;
                            }


                        }
                    }
                    else
                    {
                        totalMal++;
                    }

                    Farmacia f = new Farmacia(nombreDocumental, direccionCompleta, nombreFarmacia, codigoPostal, provincia, dirAbreviada, ciudad, urlInfo, c, telefono, fax, email, web, adicional);
                    farmaciasList.Add(f);
                }
            }
            else
            {
                Console.WriteLine("No se ha podido obtener las farmacias. Compruebe su conexión a internet.");
            }
            

            //Indica cuantas URLs están mal
            Console.WriteLine("Hay " + totalMal + " URL/XML mal.");
           

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
                    string region = node.ChildNodes[7].InnerText;

                    //Cartera de servicios
                    //Haré un preprocesado de lo que obtengo del XML
                    string cartera = node.ChildNodes[8].InnerText;
                    string[] servicios;
                    cartera.Replace('-', ' ');
                    if (cartera.IndexOf(",") == -1)
                    {
                        servicios = cartera.Split(' ');
                    }
                    else
                    {
                        servicios = cartera.Split(',');
                    }

                    for (int i = 0; i < servicios.Length; i++)
                    {
                        servicios[i] = servicios[i].Trim(' ');
                    }

                    //La calle del hospital
                    string calle = node.ChildNodes[10].InnerText;

                    //Ciudad
                    string ciudad = node.ChildNodes[11].InnerText;

                    //Obtener info extra
                    //De la URL que se nos da
                    XmlDocument masInfo = this.descargaDeURL(node.ChildNodes[14].InnerText);
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
                        // Console.WriteLine(masInfo.ChildNodes[1].Name);

                        //latitud y longitud
                        latitud = double.Parse(
                            masInfo.ChildNodes[1].ChildNodes[4].ChildNodes[6].ChildNodes[0].InnerText,
                            CultureInfo.InvariantCulture);
                        longitud =
                            double.Parse(masInfo.ChildNodes[1].ChildNodes[4].ChildNodes[6].ChildNodes[0].InnerText,
                                CultureInfo.InvariantCulture);

                        //telefono, web, email y fax (si tiene)
                        if (!masInfo.ChildNodes[1].ChildNodes[5].ChildNodes[0].InnerText.Equals(""))
                        {
                            telefono = long.Parse(masInfo.ChildNodes[1].ChildNodes[5].ChildNodes[0].InnerText);
                        }

                        //Fax (si tiene)

                        if (!masInfo.ChildNodes[1].ChildNodes[5].ChildNodes[1].InnerText.Equals(""))
                        {
                            fax = long.Parse(masInfo.ChildNodes[1].ChildNodes[5].ChildNodes[1].InnerText);
                        }

                        //Email (si tiene)

                        if (!masInfo.ChildNodes[1].ChildNodes[5].ChildNodes[2].InnerText.Equals(""))
                        {
                            email = masInfo.ChildNodes[1].ChildNodes[5].ChildNodes[2].InnerText;
                        }

                        //Web (si tiene)

                        if (!masInfo.ChildNodes[1].ChildNodes[5].ChildNodes[3].InnerText.Equals(""))
                        {
                            web = masInfo.ChildNodes[1].ChildNodes[5].ChildNodes[3].InnerText;
                        }

                    }

                    //Añado a la lista
                    this.hospitalList.Add(new Hospital(nombreHospital, dirCompleta, codigoDeHospital, codigoPostal,
                        region, servicios, calle, ciudad, new Coordenadas(latitud, longitud), email, web, telefono, fax));

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
                    string codigoCentro = node.ChildNodes[4].InnerText;

                    //Codigo postal
                    int codigoPostal;
                    if (node.ChildNodes[6].InnerText.Equals("NULL"))
                    {
                        codigoPostal = 00000;
                    }
                    else
                    {
                        codigoPostal = int.Parse(node.ChildNodes[6].InnerText.Trim());
                    }


                    //Provincia
                    string provincia = node.ChildNodes[7].InnerText;

                    //Region
                    string region = node.ChildNodes[8].InnerText;

                    //Cartera de servicios
                    //Haré un preprocesado de lo que obtengo del XML
                    string cartera = node.ChildNodes[9].InnerText;
                    string[] servicios = null;
                    if (cartera != null)
                    {
                        cartera.Replace('-', ' ');
                        cartera.Replace('.', ' ');

                        if (cartera.IndexOf(",") == -1)
                        {
                            servicios = cartera.Split(' ');
                        }
                        else
                        {
                            servicios = cartera.Split(',');
                        }

                        for (int i = 0; i < servicios.Length; i++)
                        {
                            servicios[i] = servicios[i].Trim(' ');
                        }

                    }

                    //Horario
                    string horario;

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


                    //La calle del hospital
                    string calle = node.ChildNodes[11].InnerText;

                    //Ciudad
                    string ciudad = node.ChildNodes[13].InnerText;

                    //URL adicional
                    string url = node.ChildNodes[14].InnerText;

                    //Obtener info extra
                    //De la URL que se nos da
                    XmlDocument masInfo = this.descargaDeURL(node.ChildNodes[16].InnerText);

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

                        //telefono, web, email y fax (si tiene)
                        if (!masInfo.ChildNodes[1].ChildNodes[5].ChildNodes[0].InnerText.Equals(""))
                        {
                            if (masInfo.ChildNodes[1].ChildNodes[5].ChildNodes[0].InnerText.Length > 9)
                            {
                                telefono =
                                    long.Parse(masInfo.ChildNodes[1].ChildNodes[5].ChildNodes[0].InnerText.Substring(0,
                                        9));
                            }
                            else
                            {
                                telefono = long.Parse(masInfo.ChildNodes[1].ChildNodes[5].ChildNodes[0].InnerText);
                            }

                        }

                        //Fax (si tiene)

                        if (!masInfo.ChildNodes[1].ChildNodes[5].ChildNodes[1].InnerText.Equals(""))
                        {
                            fax = long.Parse(masInfo.ChildNodes[1].ChildNodes[5].ChildNodes[1].InnerText);
                        }

                        //Email (si tiene)

                        if (!masInfo.ChildNodes[1].ChildNodes[5].ChildNodes[3].InnerText.Equals(""))
                        {
                            email = masInfo.ChildNodes[1].ChildNodes[5].ChildNodes[3].InnerText;
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
                    this.centroSaludList.Add(new CentroSalud(nombreCentro, dirCompleta, codigoCentro, codigoPostal,
                        provincia, region, servicios, horario, calle, ciudad, url, new Coordenadas(latitud, longitud),
                        email, web, telefono, fax));


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
       /* private void bicicletas()
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

                    puntosBicisList.Add(new PuntoBici(id, nombre, estado, new Coordenadas(latitud, longitud), anclajesLibres, anclajesAveriados,
                        anclajesUsados, bicisLibres, bicisAveriadas));
                }
            }
            else
            {
                Console.WriteLine("No se ha podido obtener la información de bicicletas. Compruebe su conexión a internet.");
            }

        }*/

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
            cliente.download("MetroBilbao/google_transit.zip", @"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ExtractorDatos\MetroFTP\google_transit.zip");

            Console.WriteLine("¡Completado!");

            //Procesar los archivos
            Console.WriteLine("Extranyendo...");

            ZipFile zf = null;
            try
            {
                //Establezco el fichero a descomprimir
                FileStream fs = File.OpenRead(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ExtractorDatos\MetroFTP\google_transit.zip");
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
                        String fullZipToPath = Path.Combine(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ExtractorDatos\MetroFTP\", entryFileName);
                        string directoryName = Path.GetDirectoryName(fullZipToPath);
                        if (directoryName.Length > 0)
                            Directory.CreateDirectory(directoryName);

                        using (FileStream streamWriter = File.Create(fullZipToPath))
                        {
                            StreamUtils.Copy(zipStream, streamWriter, buffer);
                        }
                        zipStream.Close();
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
                   new System.IO.StreamReader(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ExtractorDatos\MetroFTP\stops.txt");
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
                    paradasMetro.Add(id, new ParadaMetro(id, codigo, nombreParada, c, tipoLocalizacion, idParadaPadre));

                }

                file.Close();

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
            cliente.download("Euskotren/google_transit.zip", @"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ExtractorDatos\EuskotrenFTP\google_transit.zip");

            Console.WriteLine("¡Completado!");

            //Procesar los archivos
            Console.WriteLine("Extranyendo...");

            ZipFile zf = null;
            try
            {
                //Establezco el fichero a descomprimir
                FileStream fs = File.OpenRead(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ExtractorDatos\EuskotrenFTP\google_transit.zip");
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
                        String fullZipToPath = Path.Combine(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ExtractorDatos\EuskotrenFTP\", entryFileName);
                        string directoryName = Path.GetDirectoryName(fullZipToPath);
                        if (directoryName.Length > 0)
                            Directory.CreateDirectory(directoryName);

                        using (FileStream streamWriter = File.Create(fullZipToPath))
                        {
                            StreamUtils.Copy(zipStream, streamWriter, buffer);
                        }
                        zipStream.Close();
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
                   new System.IO.StreamReader(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ExtractorDatos\EuskotrenFTP\stops.txt");
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
                    paradasEuskotren.Add(id, new ParadaEuskotren(id, codigo, nombreParada, descripcion, c, idZona, urlParada, tipoLocalizacion));


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
                    @"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ExtractorDatos\BilbobusFTP\google_transit.zip"))
            {
                FTP cliente = new FTP("ftp://ftp.geo.euskadi.net/cartografia/Transporte/Moveuskadi/", "", "");
                cliente.download("Bilbobus/google_transit.zip",
                    @"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ExtractorDatos\BilbobusFTP\google_transit.zip");
            }

            Console.WriteLine("¡Completado!");

            //Procesar los archivos
            Console.WriteLine("Extranyendo...");

            ZipFile zf = null;
            try
            {
                //Establezco el fichero a descomprimir
                FileStream fs;
                try
                {
                    fs = File.OpenRead(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ExtractorDatos\BilbobusFTP\google_transit.zip");
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
                        String fullZipToPath = Path.Combine(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ExtractorDatos\BilbobusFTP\", entryFileName);
                        string directoryName = Path.GetDirectoryName(fullZipToPath);
                        if (directoryName.Length > 0)
                            Directory.CreateDirectory(directoryName);

                        using (FileStream streamWriter = File.Create(fullZipToPath))
                        {
                            StreamUtils.Copy(zipStream, streamWriter, buffer);
                        }
                        zipStream.Close();
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
                   new System.IO.StreamReader(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ExtractorDatos\BilbobusFTP\routes.txt");
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
                    lineasBilbo.Add(id, new LineaBilbobus(id, idAgencia, abreviatura, nombre, tipo));

                }

                file.Close();

                Console.WriteLine("¡Completado rutas!");
                Console.WriteLine("Leyendo viajes...");

                // Leemos el fichero linea por linea para sacar los viajes
                List<Clases.KeyValuePair<int, ViajeBilbobus>> viajes = new List<Clases.KeyValuePair<int, ViajeBilbobus>>();
                file = new System.IO.StreamReader(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ExtractorDatos\BilbobusFTP\stop_times.txt");
                primeraLinea = file.ReadLine();
                while ((line = file.ReadLine()) != null)
                {

                    //Obtengo la info que me interesa
                    string[] valoresLinea = line.Split(',');

                    //ID de viaje
                    int id = int.Parse(valoresLinea[0]);

                    //ID de la parada
                    int idParada = int.Parse(valoresLinea[3]);

                    //Comprobamos si contiene la clave
                    bool encontrado = false;
                    int index = 0;
                    foreach (Clases.KeyValuePair<int, ViajeBilbobus> temporal in viajes)
                    {
                        if (temporal.Key == id)
                        {
                            encontrado = true;
                            break;
                        }
                        else
                        {
                            index++;
                        }
                    }

                    if (encontrado)
                    {
                        //Se añade solo la parada al viaje
                        viajes[index].Value.paradas.Add(new Clases.KeyValuePair<int, ParadaBilbo>(idParada,paradasBilbobus[idParada] ));
                        viajes[index].Value.clavesParada.Add(idParada);

                    }
                    else
                    {
                        //Se añade el viaje completo
                        viajes.Add(new Clases.KeyValuePair<int, ViajeBilbobus>(id, new ViajeBilbobus(id, paradasBilbobus[idParada])));
                    }
                }

                file.Close();

                Console.WriteLine("¡Completado viajes!");
                Console.WriteLine("Juntando resultados...");

                // Leemos el fichero linea por linea
                file = new System.IO.StreamReader(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ExtractorDatos\BilbobusFTP\trips.txt");
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
                        int contador = 0;
                        bool encontrado = false;
                        foreach (Clases.KeyValuePair<int, ViajeBilbobus> temporal in viajes)
                        {
                            if (temporal.Key == idViaje)
                            {
                                encontrado = true;
                                break;
                            }
                            else
                            {
                                contador++;
                            }
                        }
                        if (encontrado)
                        {
                            lineasBilbo[idRuta].viajes.Add(viajes[contador]); 
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
                    @"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ExtractorDatos\EuskotrenFTP\google_transit.zip"))
            {
                FTP cliente = new FTP("ftp://ftp.geo.euskadi.net/cartografia/Transporte/Moveuskadi/", "", "");
                cliente.download("Euskotren/google_transit.zip", @"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ExtractorDatos\EuskotrenFTP\google_transit.zip");
            }
            

            Console.WriteLine("¡Completado!");

            //Procesar los archivos
            Console.WriteLine("Extranyendo...");

            ZipFile zf = null;
            try
            {
                //Establezco el fichero a descomprimir
                FileStream fs = File.OpenRead(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ExtractorDatos\EuskotrenFTP\google_transit.zip");
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
                        String fullZipToPath = Path.Combine(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ExtractorDatos\EuskotrenFTP\", entryFileName);
                        string directoryName = Path.GetDirectoryName(fullZipToPath);
                        if (directoryName.Length > 0)
                            Directory.CreateDirectory(directoryName);

                        using (FileStream streamWriter = File.Create(fullZipToPath))
                        {
                            StreamUtils.Copy(zipStream, streamWriter, buffer);
                        }
                        zipStream.Close();
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
                   new System.IO.StreamReader(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ExtractorDatos\EuskotrenFTP\routes.txt");
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
                    lineasEusko.Add(id, new LineaEuskotren(id, idAgencia, abreviatura, nombre, tipo));

                }

                file.Close();

                Console.WriteLine("¡Completado rutas!");
                Console.WriteLine("Leyendo viajes...");

                // Leemos el fichero linea por linea para sacar los viajes
                List<Clases.KeyValuePair<string, ViajeEuskotren>> viajes = new List<Clases.KeyValuePair<string, ViajeEuskotren>>();
                file = new System.IO.StreamReader(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ExtractorDatos\EuskotrenFTP\stop_times.txt");
                primeraLinea = file.ReadLine();
                while ((line = file.ReadLine()) != null)
                {

                    //Obtengo la info que me interesa
                    string[] valoresLinea = line.Split(',');

                    //ID de viaje
                    string id = valoresLinea[0];

                    //ID de la parada
                    int idParada = int.Parse(valoresLinea[3]);

                    //Comprobamos si contiene la clave
                    bool encontrado = false;
                    int index = 0;
                    foreach (Clases.KeyValuePair<string, ViajeEuskotren> temporal in viajes)
                    {
                        if (temporal.Key.Equals(id))
                        {
                            encontrado = true;
                            break;
                        }
                        else
                        {
                            index++;
                        }
                    }

                    if (encontrado)
                    {
                        //Se añade solo la parada al viaje
                        viajes[index].Value.paradas.Add(new Clases.KeyValuePair<int, ParadaEuskotren>(idParada, paradasEuskotren[idParada]));
                        viajes[index].Value.clavesParada.Add(idParada);

                    }
                    else
                    {
                        //Se añade el viaje completo
                        viajes.Add(new Clases.KeyValuePair<string, ViajeEuskotren>(id, new ViajeEuskotren(id, paradasEuskotren[idParada])));
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
                file = new System.IO.StreamReader(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ExtractorDatos\EuskotrenFTP\trips.txt");
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
                        int contador = 0;
                        bool encontrado = false;
                        foreach (Clases.KeyValuePair<string, ViajeEuskotren> temporal in viajes)
                        {
                            if (temporal.Key.Equals(idViaje))
                            {
                                encontrado = true;
                                break;
                            }
                            else
                            {
                                contador++;
                            }
                        }
                        if (encontrado)
                        {
                            lineasEusko[idRuta].viajes.Add(viajes[contador]);
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

        private void lineasMetro()
        {
            if (paradasBizkaibus.Count != 0)
            {
                paradasBizkaibus.Clear();
            }

            Console.WriteLine("Empiezo Lineas Metro");
            if (
                !File.Exists(
                    @"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ExtractorDatos\MetroFTP\google_transit.zip"))
            {
                FTP cliente = new FTP("ftp://ftp.geo.euskadi.net/cartografia/Transporte/Moveuskadi/", "", "");
                cliente.download("MetroBilbao/google_transit.zip",
                    @"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ExtractorDatos\MetroFTP\google_transit.zip");
            }

            Console.WriteLine("¡Completado!");

            //Procesar los archivos
            Console.WriteLine("Extranyendo...");

            ZipFile zf = null;
            try
            {
                //Establezco el fichero a descomprimir
                FileStream fs = File.OpenRead(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ExtractorDatos\MetroFTP\google_transit.zip");
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
                        String fullZipToPath = Path.Combine(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ExtractorDatos\MetroFTP\", entryFileName);
                        string directoryName = Path.GetDirectoryName(fullZipToPath);
                        if (directoryName.Length > 0)
                            Directory.CreateDirectory(directoryName);

                        using (FileStream streamWriter = File.Create(fullZipToPath))
                        {
                            StreamUtils.Copy(zipStream, streamWriter, buffer);
                        }
                        zipStream.Close();
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
                   new System.IO.StreamReader(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ExtractorDatos\MetroFTP\routes.txt");
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

                    //Color de ruta
                    string color = valoresLinea[4];

                    //Añado a la lista
                    lineasMetroBilbao.Add(id, new LineaMetro(id, abreviatura, nombre, tipo, color));
                }

                file.Close();

                Console.WriteLine("¡Completado rutas!");
                Console.WriteLine("Leyendo viajes...");

                // Leemos el fichero linea por linea para sacar los viajes
                SortedDictionary<int, ViajeMetro> viajes = new SortedDictionary<int, ViajeMetro>();
                file = new System.IO.StreamReader(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ExtractorDatos\MetroFTP\stop_times.txt");
                primeraLinea = file.ReadLine();
                while ((line = file.ReadLine()) != null)
                {

                    //Obtengo la info que me interesa
                    string[] valoresLinea = line.Split(',');

                    //ID de viaje
                    int id = int.Parse(valoresLinea[0]);

                    //Secuencia de parada
                    int secuencia = int.Parse(valoresLinea[1]);

                    //ID de la parada
                    string idParada = valoresLinea[4];

                    if (viajes.ContainsKey(id))
                    {
                        //Se añade solo al viaje la parada
                        viajes[id].paradasList.Add(new Clases.KeyValuePair<int, ParadaMetro>(secuencia, paradasMetro[idParada]));

                    }
                    else
                    {
                        //Se añade el viaje completo
                        viajes.Add(id, new ViajeMetro(id, paradasMetro[idParada], secuencia));
                    }
                }
                file.Close();



                foreach (System.Collections.Generic.KeyValuePair<int, ViajeMetro> temporal in viajes.ToList())
                {
                    temporal.Value.paradasList = temporal.Value.paradasList.OrderBy(v => v.Key).ToList();
                }

                //Los pongo bien los viajes
               /* foreach (ViajeMetro v in viajes.Values)
                {
                   Dictionary<string, ParadaMetro> temporal = new Dictionary<string, ParadaMetro>();
                    var list = from pair in v.paradas
                        orderby pair.Key ascending
                        select pair;
                   foreach (KeyValuePair<string, ParadaMetro> pair in list)
                   {
                       
                       temporal.Add(v.paradas[pair.Key].id, v.paradas[pair.Key]);
                   }
                    v.paradas = temporal;

                }*/

                Console.WriteLine("¡Completado viajes!");
                Console.WriteLine("Juntando resultados...");

                // Leemos el fichero linea por linea
                file = new System.IO.StreamReader(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ExtractorDatos\MetroFTP\trips.txt");
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
                        lineasMetroBilbao[idRuta].viajes.Add(new Clases.KeyValuePair<int, ViajeMetro>(idViaje, viajes[idViaje]));
                    }
                    catch (System.Collections.Generic.KeyNotFoundException e)
                    {
                        Console.WriteLine("No se encuentra el viaje de id: " + idViaje + " perteneciente a la ruta " + idRuta + "\n");
                    }
                }

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
                    @"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ExtractorDatos\BizkaibusFTP\google_transit.zip"))
            {
                FTP cliente = new FTP("ftp://ftp.geo.euskadi.net/cartografia/Transporte/Moveuskadi/", "", "");
                cliente.download("Bizkaibus/google_transit.zip", @"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ExtractorDatos\BizkaibusFTP\google_transit.zip");
            }


            Console.WriteLine("¡Completado!");

            //Procesar los archivos
            Console.WriteLine("Extranyendo...");

            ZipFile zf = null;
            try
            {
                //Establezco el fichero a descomprimir
                FileStream fs = File.OpenRead(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ExtractorDatos\BizkaibusFTP\google_transit.zip");
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
                        String fullZipToPath = Path.Combine(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ExtractorDatos\BizkaibusFTP\", entryFileName);
                        string directoryName = Path.GetDirectoryName(fullZipToPath);
                        if (directoryName.Length > 0)
                            Directory.CreateDirectory(directoryName);

                        using (FileStream streamWriter = File.Create(fullZipToPath))
                        {
                            StreamUtils.Copy(zipStream, streamWriter, buffer);
                        }
                        zipStream.Close();
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
                   new System.IO.StreamReader(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ExtractorDatos\BizkaibusFTP\routes.txt");
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
                    lineasBizkaia.Add(id, new LineaBizkaibus(id, idAgencia, abreviatura, nombre, tipo));

                }

                file.Close();

                Console.WriteLine("¡Completado rutas!");
                Console.WriteLine("Leyendo viajes...");

                // Leemos el fichero linea por linea para sacar los viajes
                List<Clases.KeyValuePair<int, ViajeBizkaibus>> viajes = new List<Clases.KeyValuePair<int, ViajeBizkaibus>>();
                file = new System.IO.StreamReader(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ExtractorDatos\BizkaibusFTP\stop_times.txt");
                primeraLinea = file.ReadLine();
                while ((line = file.ReadLine()) != null)
                {

                    //Obtengo la info que me interesa
                    string[] valoresLinea = line.Split(',');

                    //ID de viaje
                    int id = int.Parse(valoresLinea[0]);

                    //ID de la parada
                    int idParada = int.Parse(valoresLinea[3]);

                    //Se añade solo al viaje la parada
                    try
                    {
                        int contador = 0;
                        bool encontrado = false;
                        foreach (Clases.KeyValuePair<int, ViajeBizkaibus> temporal in viajes)
                        {
                            if (temporal.Key.Equals(id))
                            {
                                encontrado = true;
                                break;
                            }
                            else
                            {
                                contador++;
                            }
                        }
                        if (encontrado)
                        {
                            //lineasEusko[idRuta].viajes.Add(viajes[contador]);
                            viajes[contador].Value.paradas.Add(new Clases.KeyValuePair<int, ParadaBizkaibus>(idParada,
                                paradasBizkaibus[idParada]));
                            viajes[contador].Value.clavesParada.Add(idParada);
                        }
                        else
                        {
                            //Se añade el viaje completo
                            viajes.Add(new Clases.KeyValuePair<int, ViajeBizkaibus>(id, new ViajeBizkaibus(id, paradasBizkaibus[idParada])));
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
                file = new System.IO.StreamReader(@"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ExtractorDatos\BizkaibusFTP\trips.txt");
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
                        int contador = 0;
                        bool encontrado = false;
                        foreach (Clases.KeyValuePair<int, ViajeBizkaibus> temporal in viajes)
                        {
                            if (temporal.Key.Equals(idViaje))
                            {
                                encontrado = true;
                                break;
                            }
                            else
                            {
                                contador++;
                            }
                        }
                        if (encontrado)
                        {
                            //lineasEusko[idRuta].viajes.Add(viajes[contador]);
                            lineasBizkaia[idRuta].viajes.Add(viajes[contador]);
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
                                
                                paradasTranvia.Add(new ParadaTranvia(nombre, descripcion, new Coordenadas(latitud, longitud)));

                            }
                            
                        }
                    }
                    
                }
                foreach (ParadaTranvia temporal in paradasTranvia)
                {
                    Console.WriteLine(temporal.ToString());
                }
            }
        }

    }
}
