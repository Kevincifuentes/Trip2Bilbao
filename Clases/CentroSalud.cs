using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class CentroSalud
    {

        public CentroSalud()
        {
            
        }

        public CentroSalud(string nombreCentro, string dirCompleta, string codigoCentro, int codigoPostal, string provincia, string region, string[] servicios, string horario, string calle, string ciudad, string urlAdicional, Coordenadas localizacion, string email, string web, long telefono, long fax)
        {
            this.nombreCentro = nombreCentro;
            this.dirCompleta = dirCompleta;
            this.codigoCentro = codigoCentro;
            this.codigoPostal = codigoPostal;
            this.provincia = provincia;
            this.region = region;
            this.servicios = servicios;
            this.horario = horario;
            this.calle = calle;
            this.ciudad = ciudad;
            this.urlAdicional = urlAdicional;
            this.localizacion = localizacion;
            this.email = email;
            this.web = web;
            this.telefono = telefono;
            this.fax = fax;
        }

        public string nombreCentro { get; set; }
        public string dirCompleta { get; set; }
        public string codigoCentro { get; set; }
        public int codigoPostal { get; set; }
        public string provincia { get; set; }
        public string region { get; set; }
        public string[] servicios { get; set; }
        public string horario { get; set; }
        public string calle { get; set; }
        public string ciudad { get; set; }
        public string urlAdicional { get; set; }
        public Coordenadas localizacion { get; set; }
        public string email { get; set; }
        public string web { get; set; }
        public long telefono { get; set; }
        public long fax { get; set; }

        public override string ToString()
        {
            string respuesta = nombreCentro + " : " + "Direccion completa: " +
                                dirCompleta + " Localización: ";

            respuesta = respuesta + "CP: " + codigoPostal + " Region: " + region + " Calle: " +
                        calle + " Ciudad: " + ciudad + " Telefono: " + telefono + " Web: " + web + "\n";
            respuesta = respuesta + " Fax: " + fax + " Email: " +
                       email + "\n";
            respuesta = respuesta + "\t-------Servicios------\n";
            foreach (string s in servicios)
            {
                respuesta = respuesta + s + "\n";
            }
            return respuesta; 
        }
    }
}
