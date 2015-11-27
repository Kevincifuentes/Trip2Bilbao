using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Clases;
using Microsoft.VisualBasic.FileIO;

namespace ExtractorDatos
{
    internal class InformacionDinamica
    {
        private Dictionary<string, TiempoCiudad> tiempoPorCiudades = new Dictionary<string, TiempoCiudad>();
        private Dictionary<string, TiempoComarca> tiempoPorComarcas = new Dictionary<string, TiempoComarca>();
        private List<Obra> obras = new List<Obra>();
        private List<Incidencia> incidencias = new List<Incidencia>();
        private List<Mantenimiento> mantenimientos = new List<Mantenimiento>();
        private List<Evento> eventos = new List<Evento>();
        
        private TiempoPrediccion tiempoPrediccion;

        public InformacionDinamica(ProgramaPrincipal p)
        {
            
            int c;
            do{
                Console.WriteLine("1. Meteorología");
                Console.WriteLine("2. Tiempos Autobuses Bilbobus");
                Console.WriteLine("3. Incidencias de tráfico Co-Cities Bilbao.");
                Console.WriteLine("4. SALIR");
                c= int.Parse(Console.ReadLine());
            switch (c)
            {
                case 1:
                    Console.WriteLine("Case 1");
                    meteoMenu();
                    break;
                case 2:
                    Console.WriteLine("Case 2");
                    tiemposParadaBilbo(p);
                    ProgramaPrincipal.emisor.enviarParadasBilbobus(p.paradasBilbobus);
                    break;
                case 3:
                    Console.WriteLine("Case 3");
                    incidenciasTrafico();
                    break;
                case 4:
                    Console.WriteLine("Case 4");
                    break;
            }
             } while (c!=4);

            Console.ReadKey();
        }


        private void meteoMenu()
        {
            Console.WriteLine("1. Meteorología por Ciudades");
            Console.WriteLine("2. Meteorología por Comarca");
            Console.WriteLine("3. Meteorología: predicción proximos 6 días");
            Console.WriteLine("4. SALIR");
            char c = char.Parse(Console.ReadLine());
            switch (c)
            {
                case '1':
                    Console.WriteLine("METEO POR CIUDAD");
                    meteorologiaCiudad();
                    ProgramaPrincipal.emisor.enviarTiempoCiudad(tiempoPorCiudades);
                    break;
                case '2':
                    Console.WriteLine("METEO POR COMARCA");
                    meteorologiaComarca();
                    ProgramaPrincipal.emisor.enviarTiempoComarca(tiempoPorComarcas);
                    break;
                case '3':
                    Console.WriteLine("METEO POR PREDICCION 6 DIAS");
                    meteorologiaPrediccion();
                    ProgramaPrincipal.emisor.enviarTiempoPrediccion(tiempoPrediccion);
                    break;
            }
        }

        private XmlDocument descargaDeURL(string url)
        {

            XmlDocument document = new XmlDocument();
            try
            {
                document.Load(url);
            }
            catch (System.Net.WebException e)
            {
                Console.WriteLine("Error al intentar obtener la url: " + url);
                document = null;
            }
            catch (System.Xml.XmlException ex)
            {
                Console.WriteLine("Error al intentar obtener la url: " + url);
                //Convertir a XML el churro HTML

                document = null;
            }
            return document;
        }

        private void meteorologiaCiudad()
        {
            XmlDocument ciudadesxml =
                this.descargaDeURL(
                    "http://opendata.euskadi.eus/contenidos/prevision_tiempo/met_forecast/opendata/met_forecast.xml");
            if (ciudadesxml != null)
            {
                XmlNodeList temp = ciudadesxml.SelectNodes("//*");

                //Obtenemos la hora y fecha de actualización del tiempo
                string fechaA = temp[0].Attributes.Item(0).InnerText;
                int dianA = int.Parse(fechaA.Substring(0, 2));
                int mesA = int.Parse(fechaA.Substring(3, 2));
                int anoA = int.Parse(fechaA.Substring(6, 4));

                string horaDeActualizacion = temp[0].Attributes.Item(0).InnerText;
                int puntoInicial = horaDeActualizacion.IndexOf("[") + 1;
                int puntoFinal = horaDeActualizacion.IndexOf("]");
                horaDeActualizacion = horaDeActualizacion.Substring(puntoInicial, puntoFinal - puntoInicial);
                string[] temporal = horaDeActualizacion.Split(':');
                int hora = int.Parse(temporal[0]);
                int minuto = int.Parse(temporal[1]);
                int segundo = int.Parse(temporal[2]);

                DateTime diaActualizacion = new DateTime(anoA, mesA, dianA, hora, minuto, segundo);


                int contador = 0;
                foreach (XmlNode x in temp[0].ChildNodes[0].ChildNodes)
                {

                    //fecha (numérica) del tiempo
                    string fecha = x.Attributes.Item(1).InnerText;
                    int dian = int.Parse(fecha.Substring(0, 2));
                    int mes = int.Parse(fecha.Substring(3, 2));
                    int ano = int.Parse(fecha.Substring(6, 4));
                    DateTime dia = new DateTime(ano, mes, dian);

                    //Obtenemos la descripcion del tiempo
                    string descripcionES = x.ChildNodes[1].ChildNodes[0].InnerText;
                    string descripcionEU = x.ChildNodes[1].ChildNodes[1].InnerText;

                    //Obtenemos la fecha escrita
                    string fechaEscrita = x.ChildNodes[2].InnerText;

                    List<Clases.KeyValuePair<string, TiempoDiaCiudad>> tiempoList = new List<Clases.KeyValuePair<string, TiempoDiaCiudad>>();
                    //Información simple sobre unas pocas ciudades
                    foreach (XmlNode nodo in x.ChildNodes[4].ChildNodes)
                    {
                        //Nombre de la ciduad
                        string nombreCiudad = nodo.Attributes.Item(0).InnerText;

                        //ID de la ciudad
                        int id = int.Parse(nodo.Attributes.Item(1).InnerText);

                        //Obtener una descripción de su tiempo
                        string descCiudadES = nodo.ChildNodes[0].ChildNodes[1].ChildNodes[0].InnerText;
                        string descCiudadEU = nodo.ChildNodes[0].ChildNodes[1].ChildNodes[1].InnerText;

                        //Temperatura Max y Min
                        int max = int.Parse(nodo.ChildNodes[1].InnerText);
                        int min = int.Parse(nodo.ChildNodes[2].InnerText);

                        //Añadimos a la lista
                        tiempoList.Add(new Clases.KeyValuePair<string, TiempoDiaCiudad>(nombreCiudad, new TiempoDiaCiudad(nombreCiudad, id, descCiudadES, descCiudadEU, max, min)));

                    }

                    //Determina el día en el que nos encontramos
                    string idetificativoDia = "Desconocido";
                    if (contador == 0)
                    {
                        idetificativoDia = "Hoy";
                    }
                    else if (contador == 1)
                    {
                        idetificativoDia = "Mañana";
                    }
                    else
                    {
                        idetificativoDia = "Pasado mañana";
                    }
                    contador++;

                    //Añado a la lista del tiempo
                    tiempoPorCiudades.Add(idetificativoDia,
                        new TiempoCiudad(diaActualizacion, dia, descripcionES, descripcionEU, fechaEscrita, tiempoList));
                }
            }
            else
            {
                Console.WriteLine("No se ha podido obtener la meteorologia por ciudad. Compruebe su conexión a internet.");
            }
            


        }

        private void meteorologiaComarca()
        {
            XmlDocument comarcasxml =
                this.descargaDeURL(
                    "http://opendata.euskadi.eus/contenidos/prevision_tiempo/met_forecast_zone/opendata/met_forecast_zone.xml");
            if (comarcasxml != null)
            {
                XmlNodeList temp = comarcasxml.SelectNodes("//*");
                Console.WriteLine(temp[0].Name);

                foreach (XmlNode node in temp[0].ChildNodes)
                {
                    //Obtenemos la hora y fecha de actualización del tiempo
                    string fechaA = node.Attributes.Item(0).InnerText;
                    int dianA = int.Parse(fechaA.Substring(0, 2));
                    int mesA = int.Parse(fechaA.Substring(3, 2));
                    int anoA = int.Parse(fechaA.Substring(6, 4));

                    string horaDeActualizacion = node.Attributes.Item(0).InnerText;
                    int puntoInicial = horaDeActualizacion.IndexOf("[") + 1;
                    int puntoFinal = horaDeActualizacion.IndexOf("]");
                    horaDeActualizacion = horaDeActualizacion.Substring(puntoInicial, puntoFinal - puntoInicial);
                    string[] temporal = horaDeActualizacion.Split(':');
                    int hora = int.Parse(temporal[0]);
                    int minuto = int.Parse(temporal[1]);
                    int segundo = int.Parse(temporal[2]);

                    DateTime diaActualizacion = new DateTime(anoA, mesA, dianA, hora, minuto, segundo);

                    //Id de comarca
                    int id = int.Parse(node.Attributes.Item(1).InnerText);

                    //Nombre de la comarca
                    string nombreComarcaES = node.ChildNodes[0].ChildNodes[0].InnerText;
                    string nombreComarcaEU = node.ChildNodes[0].ChildNodes[1].InnerText;

                    //Obtenemos el tiempo para esa comarca
                    //De hoy y mañana
                    List<TiempoDiaComarca> tmpC = new List<TiempoDiaComarca>();
                    foreach (XmlNode nodeDia in node.ChildNodes[1].ChildNodes)
                    {
                        //Para cuando es la prediccion
                        string para = nodeDia.Attributes.Item(0).InnerText;
                        if (para.Equals("today"))
                        {
                            para = "Hoy";
                        }
                        else if (para.Equals("tomorrow"))
                        {
                            para = "Mañana";
                        }
                        else
                        {
                            para = "Desconocido";
                        }

                        //Día de la prediccion
                        string fechaPeriodo = nodeDia.Attributes.Item(1).InnerText;
                        int diaP = int.Parse(fechaPeriodo.Substring(0, 2));
                        int mesP = int.Parse(fechaPeriodo.Substring(3, 2));
                        int anoP = int.Parse(fechaPeriodo.Substring(6, 4));

                        //Obtenemos el tiempo
                        //Primero el viento en la comarca
                        string vientoES = nodeDia.ChildNodes[0].ChildNodes[0].ChildNodes[1].ChildNodes[0].InnerText;
                        string vientoEU = nodeDia.ChildNodes[0].ChildNodes[0].ChildNodes[1].ChildNodes[1].InnerText;

                        //despues el tiempo
                        string tiempoES = nodeDia.ChildNodes[0].ChildNodes[1].ChildNodes[1].ChildNodes[0].InnerText;
                        string tiempoEU = nodeDia.ChildNodes[0].ChildNodes[1].ChildNodes[1].ChildNodes[1].InnerText;

                        //ahora la temperatura
                        string temperaturaES = nodeDia.ChildNodes[0].ChildNodes[2].ChildNodes[1].ChildNodes[0].InnerText;
                        string temperaturaEU = nodeDia.ChildNodes[0].ChildNodes[2].ChildNodes[1].ChildNodes[1].InnerText;

                        //por ultimo una descripcion general del tiempo en la comarca
                        string descripcionES = nodeDia.ChildNodes[0].ChildNodes[3].ChildNodes[0].InnerText;
                        string descripcionEU = nodeDia.ChildNodes[0].ChildNodes[3].ChildNodes[1].InnerText;

                        //Añadimos a una lista
                        tmpC.Add(new TiempoDiaComarca(new DateTime(anoP, mesP, diaP), vientoES, vientoEU, tiempoES, tiempoEU,
                            temperaturaES, temperaturaEU, descripcionES, descripcionEU));

                    }

                    //Añadimos a la lista global
                    tiempoPorComarcas.Add(nombreComarcaES,
                        new TiempoComarca(diaActualizacion, nombreComarcaES, nombreComarcaEU, tmpC));
                }
            }
            else
            {
                Console.WriteLine("No se ha podido obtener la meteorologia por comarca. Compruebe su conexión a internet.");
            }
            

        }

        private void meteorologiaPrediccion()
        {
            XmlDocument prediccionxml =
                this.descargaDeURL(
                    "http://opendata.euskadi.eus/contenidos/tendencias/met_tendency/opendata/met_tendency.xml");
            if (prediccionxml != null)
            {
                XmlNodeList temp = prediccionxml.SelectNodes("//*");

                //Obtengo hasta cuando dura la prediccion
                string fechaFin = temp[0].ChildNodes[0].InnerText;
                int diaF = int.Parse(fechaFin.Substring(0, 2));
                int mesF = int.Parse(fechaFin.Substring(3, 2));
                int anoF = int.Parse(fechaFin.Substring(6, 4));
                DateTime fechaDeFin = new DateTime(anoF, mesF, diaF);

                //Obtengo fecha de realizacion de la prediccion
                string fechaR = temp[0].ChildNodes[1].InnerText;
                int diaR = int.Parse(fechaR.Substring(0, 2));
                int mesR = int.Parse(fechaR.Substring(3, 2));
                int anoR = int.Parse(fechaR.Substring(6, 4));
                DateTime fechaRealizacion = new DateTime(anoF, mesF, diaF);

                List<TiempoDia> diasTemp = new List<TiempoDia>();
                //Empiezo a procesar los dias distintos
                for (int i = 2; i < 8; i++)
                {
                    //Numero de prediccion
                    int num = int.Parse(temp[0].ChildNodes[i].Attributes.Item(0).InnerText);

                    //Fecha prediccion
                    string fechaP = temp[0].ChildNodes[i].Attributes.Item(1).InnerText;
                    int diaP = int.Parse(fechaP.Substring(0, 2));
                    int mesP = int.Parse(fechaP.Substring(3, 2));
                    int anoP = int.Parse(fechaP.Substring(6, 4));
                    DateTime fechaPrediccion = new DateTime(anoP, mesP, diaP);

                    //EMPIEZO a obtener la información
                    //Informacion del viento
                    string vientoES = temp[0].ChildNodes[i].ChildNodes[0].ChildNodes[1].ChildNodes[0].InnerText;
                    string vientoEU = temp[0].ChildNodes[i].ChildNodes[0].ChildNodes[1].ChildNodes[1].InnerText;

                    //Informacion del tiempo
                    string tiempoES = temp[0].ChildNodes[i].ChildNodes[1].ChildNodes[1].ChildNodes[0].InnerText;
                    string tiempoEU = temp[0].ChildNodes[i].ChildNodes[1].ChildNodes[1].ChildNodes[1].InnerText;

                    //Informacion de temperaturas
                    string temperaturaES = temp[0].ChildNodes[i].ChildNodes[2].ChildNodes[1].ChildNodes[0].InnerText;
                    string temperaturaEU = temp[0].ChildNodes[i].ChildNodes[2].ChildNodes[1].ChildNodes[1].InnerText;

                    //Creo y añado a la lista
                    diasTemp.Add(new TiempoDia(fechaPrediccion, vientoES, vientoEU, tiempoES, tiempoEU, temperaturaES,
                        temperaturaEU));
                }

                //Obtengo cuando empieza la prediccion
                string fechaInicio = temp[0].ChildNodes[8].InnerText;
                int diaI = int.Parse(fechaInicio.Substring(0, 2));
                int mesI = int.Parse(fechaInicio.Substring(3, 2));
                int anoI = int.Parse(fechaInicio.Substring(6, 4));
                DateTime fechaDeInicio = new DateTime(anoF, mesF, diaF);

                //Obtengo fecha de obtención (descarga) de la información
                string fechaObtencion = temp[0].ChildNodes[9].InnerText;

                tiempoPrediccion = new TiempoPrediccion(fechaDeFin, fechaDeInicio, fechaRealizacion, diasTemp,
                    fechaObtencion);
            }
            else
            {
                Console.WriteLine("No se ha podido obtener la predicción meteorológica para 6 días. Compruebe su conexión a internet.");
            }
            

        }

        private void tiemposParadaBilbo(ProgramaPrincipal p)
        {
            //Descargar CSV
            string url = "http://www.bilbao.net/autobuses/jsp/od_horarios.jsp?idioma=c&formato=csv&tipo=espera";
            bool correcto=true;
            using (var client = new WebClient())
            using (

                var file =
                    File.Create(
                        @"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ExtractorDatos\BilboBus\BilboBusTiempos.csv")
                )
            {
                try
                {
                    var bytes = client.DownloadData(url);
                    file.Write(bytes, 0, bytes.Length);
                }
                catch(System.Net.WebException ex)
                {
                    correcto = false;
                    Console.WriteLine("No se ha podido obtener los tiempos por parada de Bilbobus. Compruebe su conexión a internet.");
                }
                
            }
            if(correcto)
            {
                //Procesar CSV
                TextFieldParser parser =
                    new TextFieldParser(
                        @"C:\Users\Kevin\Documents\visual studio 2013\Projects\ExtractorDatos\ExtractorDatos\BilboBus\BilboBusTiempos.csv");
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                bool primera = true;
                while (!parser.EndOfData)
                {
                    //Procesando fila
                    string[] fields = parser.ReadFields();

                    foreach (string field in fields)
                    {
                        if (!primera)
                        {
                            //TODO: Process field
                            //Console.WriteLine("[" + field.ToString() + "]");
                            string[] campos = field.Split(';');

                            //Codigo de linea
                            string codigoLinea = campos[0];

                            //Descripcion Linea (Nombre)
                            string descripcionLinea = campos[1];

                            //Codigo parada
                            int codigoParada = int.Parse(campos[2]);

                            //Nombre parada
                            string nombreParada = campos[3];

                            //Tiempo de espera para esa linea en esa parada
                            int tiempoEspera = 00;
                            try {
                                tiempoEspera = int.Parse(campos[4]);
                            }
                            catch(System.FormatException ex)
                            {
                                Console.WriteLine("Tiempo de espera invalido en ID: " + codigoParada);
                            }
                            

                            ParadaBilbo p2 = p.paradasBilbobus[codigoParada];
                            int index = -1;
                            int contador = 0;
                            foreach (Clases.KeyValuePair<string, LineaBusTiempo> temporal in p2.lineasYTiempo)
                            {
                                if (temporal.comprobarKey(codigoLinea) == 1)
                                {
                                    index = contador;
                                }
                                else
                                {
                                    contador++;
                                }
                            }
                            if (index != -1)
                            {
                                p2.lineasYTiempo[index].Value.tiempoEspera = tiempoEspera;
                            }
                            else
                            {
                                p2.lineasYTiempo.Add(new Clases.KeyValuePair<string, LineaBusTiempo>(codigoLinea,new LineaBusTiempo(codigoLinea, descripcionLinea, tiempoEspera) ));
                            }
                        }
                        else
                        {
                            primera = false;
                        }
                    }
                }
                parser.Close(); 
            }
            
        }

        private void incidenciasTrafico()
        {

            Console.WriteLine("1. Eventos que afectan al tráfico.");
            Console.WriteLine("2. Obra que afecta al tráfico.");
            Console.WriteLine("3. Cualquier incidencia que afecta al tráfico.");
            Console.WriteLine("4. Actuación de mantenimiento que afecta al tráfico.");
            Console.WriteLine("5. SALIR");
            char c = char.Parse(Console.ReadLine());
            switch (c)
            {
                case '1':
                    Console.WriteLine("Eventos");
                    eventosTrafico();
                    ProgramaPrincipal.emisor.enviarEventosTrafico(eventos);
                    foreach (Evento e in eventos)
                    {
                        Console.WriteLine(e);
                    }
                    break;
                case '2':
                    Console.WriteLine("Obras");
                    obrasTrafico();
                    ProgramaPrincipal.emisor.enviarObrasTrafico(obras);
                    foreach (Obra o in obras)
                    {
                        Console.WriteLine(o);
                    }
                    break;
                case '3':
                    Console.WriteLine("Incidencias");
                    incidenciasVariasTrafico();
                    ProgramaPrincipal.emisor.enviarIncidenciasTrafico(incidencias);
                    foreach (Incidencia i in incidencias)
                    {
                        Console.WriteLine(i);
                    }
                    break;
                case '4':
                    Console.WriteLine("Mantenimiento");
                    mantenimientoTrafico();
                    ProgramaPrincipal.emisor.enviarMantenimientosTrafico(mantenimientos);
                    foreach (Mantenimiento m in mantenimientos)
                    {
                        Console.WriteLine(m);
                    }
                    break;
            }
        }

        private void eventosTrafico()
        {
            XmlDocument eventosxml =
                this.descargaDeURL("http://www.geobilbao.net/wfsCocities?service=wfs&version=1.1.0&request=GetFeature&typeName=eti:Activities");

            if (eventosxml != null)
            {
                XmlNodeList temp = eventosxml.SelectNodes("//*");

                if (temp[0].ChildNodes[0].HasChildNodes)
                {
                    //Itero por los nodos
                    foreach (XmlNode temporal in temp[0].ChildNodes[0].ChildNodes)
                    {
                        //Obtengo el id del evento
                        string atributo = temporal.Attributes[0].InnerText;
                        int inicio = atributo.IndexOf(":");
                        int id = int.Parse(atributo.Substring(inicio + 1, atributo.Length - inicio - 1));

                        //Día de inicio/creación
                        string fecha = temporal.ChildNodes[0].InnerText;
                        int ano = int.Parse(fecha.Substring(0, 4));
                        int mes = int.Parse(fecha.Substring(5, 2));
                        int dia = int.Parse(fecha.Substring(8, 2));
                        int hora = int.Parse(fecha.Substring(11, 2));
                        int minutos = int.Parse(fecha.Substring(14, 2));
                        int segundos = int.Parse(fecha.Substring(17, 2));
                        DateTime fechaInicio = new DateTime(ano, mes, dia, hora, minutos, segundos);

                        //Dia de finalización
                        fecha = temporal.ChildNodes[1].InnerText;
                        ano = int.Parse(fecha.Substring(0, 4));
                        mes = int.Parse(fecha.Substring(5, 2));
                        dia = int.Parse(fecha.Substring(8, 2));
                        hora = int.Parse(fecha.Substring(11, 2));
                        minutos = int.Parse(fecha.Substring(14, 2));
                        segundos = int.Parse(fecha.Substring(17, 2));
                        DateTime fechaFin = new DateTime(ano, mes, dia, hora, minutos, segundos);

                        //Descripcion Obra
                        string descripcionEvento =
                            temporal.ChildNodes[2].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0]
                                .InnerText;

                        //Localización
                        string latlong =
                            temporal.ChildNodes[3].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText;
                        string[] aLatLong = latlong.Split(' ');

                        double longitud = double.Parse(aLatLong[0], CultureInfo.InvariantCulture);
                        double latitud = double.Parse(aLatLong[1], CultureInfo.InvariantCulture);

                        Coordenadas c = new Coordenadas(latitud, longitud);

                        //Añadir Evento a la lista
                        eventos.Add(new Evento(id, fechaInicio, fechaFin, descripcionEvento, c));
                    }
                }
                else
                {
                    Console.WriteLine("\t No hay eventos para mostrar.");
                }
            }
            else
            {
                Console.WriteLine("No se ha podido obtener los eventos del tráfico. Compruebe su conexión a internet.");
            }
           


        }


        private void obrasTrafico()
        {
            XmlDocument obrasxml =
                this.descargaDeURL(
                    "http://www.geobilbao.net/wfsCocities?service=wfs&version=1.1.0&request=GetFeature&typeName=eti:ConstructionWorks");

            if (obrasxml != null)
            {
                XmlNodeList temp = obrasxml.SelectNodes("//*");
                if (temp[0].ChildNodes[0].HasChildNodes)
                {
                    //Itero por los nodos
                    foreach (XmlNode temporal in temp[0].ChildNodes[0].ChildNodes)
                    {
                        //Obtengo el id de la obra
                        string atributo = temporal.Attributes[0].InnerText;
                        int inicio = atributo.IndexOf(":");
                        int id = int.Parse(atributo.Substring(inicio + 1, atributo.Length - inicio - 1));

                        //Día de inicio/creación
                        string fecha = temporal.ChildNodes[0].InnerText;
                        int ano = int.Parse(fecha.Substring(0, 4));
                        int mes = int.Parse(fecha.Substring(5, 2));
                        int dia = int.Parse(fecha.Substring(8, 2));
                        int hora = int.Parse(fecha.Substring(11, 2));
                        int minutos = int.Parse(fecha.Substring(14, 2));
                        int segundos = int.Parse(fecha.Substring(17, 2));
                        DateTime fechaInicio = new DateTime(ano, mes, dia, hora, minutos, segundos);

                        //Dia de finalización
                        fecha = temporal.ChildNodes[1].InnerText;
                        ano = int.Parse(fecha.Substring(0, 4));
                        mes = int.Parse(fecha.Substring(5, 2));
                        dia = int.Parse(fecha.Substring(8, 2));
                        hora = int.Parse(fecha.Substring(11, 2));
                        minutos = int.Parse(fecha.Substring(14, 2));
                        segundos = int.Parse(fecha.Substring(17, 2));
                        DateTime fechaFin = new DateTime(ano, mes, dia, hora, minutos, segundos);

                        //Descripcion Obra
                        string descripcionObra =
                            temporal.ChildNodes[2].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0]
                                .InnerText;

                        //Localización
                        string latlong =
                            temporal.ChildNodes[3].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText;
                        string[] aLatLong = latlong.Split(' ');

                        double longitud = double.Parse(aLatLong[0], CultureInfo.InvariantCulture);
                        double latitud = double.Parse(aLatLong[1], CultureInfo.InvariantCulture);

                        Coordenadas c = new Coordenadas(latitud, longitud);

                        //Añadir Obra a la lista
                        obras.Add(new Obra(id, fechaInicio, fechaFin, descripcionObra, c));
                    }

                }
                else
                {
                    Console.WriteLine("\t No hay obras para mostrar.");
                }
            }
            else
            {
                Console.WriteLine("No se ha podido obtener las obras que afectan al tráfico. Compruebe su conexión a internet.");
            }
        }


        private void incidenciasVariasTrafico()
        {
            XmlDocument incidenciasxml =
                this.descargaDeURL(
                    "http://www.geobilbao.net/wfsCocities?service=wfs&version=1.1.0&request=GetFeature&typeName=eti:GeneralObstruction");
            if (incidenciasxml != null)
            {
                XmlNodeList temp = incidenciasxml.SelectNodes("//*");
                if (temp[0].ChildNodes[0].HasChildNodes)
                {
                    //Itero por los nodos
                    foreach (XmlNode temporal in temp[0].ChildNodes[0].ChildNodes)
                    {
                        //Obtengo el id de la incidencia
                        string atributo = temporal.Attributes[0].InnerText;
                        int inicio = atributo.IndexOf(":");
                        int id = int.Parse(atributo.Substring(inicio + 1, atributo.Length - inicio - 1));

                        //Día de inicio/creación
                        string fecha = temporal.ChildNodes[0].InnerText;
                        int ano = int.Parse(fecha.Substring(0, 4));
                        int mes = int.Parse(fecha.Substring(5, 2));
                        int dia = int.Parse(fecha.Substring(8, 2));
                        int hora = int.Parse(fecha.Substring(11, 2));
                        int minutos = int.Parse(fecha.Substring(14, 2));
                        int segundos = int.Parse(fecha.Substring(17, 2));
                        DateTime fechaInicio = new DateTime(ano, mes, dia, hora, minutos, segundos);

                        //Dia de finalización
                        fecha = temporal.ChildNodes[1].InnerText;
                        ano = int.Parse(fecha.Substring(0, 4));
                        mes = int.Parse(fecha.Substring(5, 2));
                        dia = int.Parse(fecha.Substring(8, 2));
                        hora = int.Parse(fecha.Substring(11, 2));
                        minutos = int.Parse(fecha.Substring(14, 2));
                        segundos = int.Parse(fecha.Substring(17, 2));
                        DateTime fechaFin = new DateTime(ano, mes, dia, hora, minutos, segundos);

                        //Descripcion incidencia
                        string descripcionIncidencia =
                            temporal.ChildNodes[2].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0]
                                .InnerText;

                        //Localización
                        string latlong =
                            temporal.ChildNodes[3].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText;
                        string[] aLatLong = latlong.Split(' ');

                        double longitud = double.Parse(aLatLong[0], CultureInfo.InvariantCulture);
                        double latitud = double.Parse(aLatLong[1], CultureInfo.InvariantCulture);

                        Coordenadas c = new Coordenadas(latitud, longitud);

                        //Añadir incidencia a la lista
                        incidencias.Add(new Incidencia(id, fechaInicio, fechaFin, descripcionIncidencia, c));
                    }
                }
                else
                {
                    Console.WriteLine("\t No hay incidencias para mostrar.");
                }
            }
            else
            {
                Console.WriteLine("No se ha podido obtener las incidencias que afectan al tráfico. Compruebe su conexión a internet.");
            }
            
            
        }

        private void mantenimientoTrafico()
        {
            XmlDocument mantenimientoxml =
                this.descargaDeURL(
                    "http://www.geobilbao.net/wfsCocities?service=wfs&version=1.1.0&request=GetFeature&typeName=eti:MaintenanceWorks");

            if (mantenimientoxml != null)
            {
                XmlNodeList temp = mantenimientoxml.SelectNodes("//*");

                if (temp[0].ChildNodes[0].HasChildNodes)
                {
                    //Itero por los nodos
                    foreach (XmlNode temporal in temp[0].ChildNodes[0].ChildNodes)
                    {
                        //Obtengo el id del mantenimiento
                        string atributo = temporal.Attributes[0].InnerText;
                        int inicio = atributo.IndexOf(":");
                        int id = int.Parse(atributo.Substring(inicio + 1, atributo.Length - inicio - 1));

                        //Día de inicio/creación
                        string fecha = temporal.ChildNodes[0].InnerText;
                        int ano = int.Parse(fecha.Substring(0, 4));
                        int mes = int.Parse(fecha.Substring(5, 2));
                        int dia = int.Parse(fecha.Substring(8, 2));
                        int hora = int.Parse(fecha.Substring(11, 2));
                        int minutos = int.Parse(fecha.Substring(14, 2));
                        int segundos = int.Parse(fecha.Substring(17, 2));
                        DateTime fechaInicio = new DateTime(ano, mes, dia, hora, minutos, segundos);

                        //Dia de finalización
                        fecha = temporal.ChildNodes[1].InnerText;
                        ano = int.Parse(fecha.Substring(0, 4));
                        mes = int.Parse(fecha.Substring(5, 2));
                        dia = int.Parse(fecha.Substring(8, 2));
                        hora = int.Parse(fecha.Substring(11, 2));
                        minutos = int.Parse(fecha.Substring(14, 2));
                        segundos = int.Parse(fecha.Substring(17, 2));
                        DateTime fechaFin = new DateTime(ano, mes, dia, hora, minutos, segundos);

                        //Descripcion mantenimiento
                        string descripcionMantenimiento =
                            temporal.ChildNodes[2].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0]
                                .InnerText;

                        //Localización
                        string latlong =
                            temporal.ChildNodes[3].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText;
                        string[] aLatLong = latlong.Split(' ');

                        double longitud = double.Parse(aLatLong[0], CultureInfo.InvariantCulture);
                        double latitud = double.Parse(aLatLong[1], CultureInfo.InvariantCulture);

                        Coordenadas c = new Coordenadas(latitud, longitud);

                        //Añadir mantenimiento a la lista
                        mantenimientos.Add(new Mantenimiento(id, fechaInicio, fechaFin, descripcionMantenimiento, c));
                    }
                }
                else
                {
                    Console.WriteLine("\t No hay mantenimientos para mostrar.");
                } 
            }
            else
            {
                Console.WriteLine("No se ha podido obtener los mantenimientos que afectan al tráfico. Compruebe su conexión a internet.");
            }
        }
    }
}
