using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class Hospital
    {
        public Hospital()
        {
            
        }

        public Hospital(string nombreHospital, string direccionCompleta, string codigoHospital, int codigoPostal, string region, string[] servicios, string calle, string ciudad, Coordenadas localizacion, string email, string web, long telefono, long fax)
        {
            this.nombreHospital = nombreHospital;
            this.direccionCompleta = direccionCompleta;
            this.codigoHospital = codigoHospital;
            this.codigoPostal = codigoPostal;
            this.region = region;
            this.servicios = servicios;
            this.calle = calle;
            this.ciudad = ciudad;
            this.localizacion = localizacion;
            this.email = email;
            this.web = web;
            this.telefono = telefono;
            this.fax = fax;
        }

        public string nombreHospital { get; set; }
        public string direccionCompleta { get; set; }
        public string codigoHospital { get; set; }
        public int codigoPostal { get; set; }
        public string region { get; set; }
        public string[] servicios { get; set; }
        public string calle { get; set; }
        public string ciudad { get; set; }
        public Coordenadas localizacion { get; set; }
        public string email { get; set; }
        public string web { get; set; }
        public long telefono { get; set; }
        public long fax { get; set; }

        public override string ToString()
        {
            string respuesta = nombreHospital + " : "+ "Direccion completa: " +
                               direccionCompleta + " Localización: ";

            respuesta = respuesta + "CP: " + codigoPostal + " Region: " + region + " Calle: " +
                        calle+" Ciudad: "+ciudad + " Telefono: " + telefono + " Web: " + web + "\n";
            respuesta = respuesta + " Fax: " + fax + " Email: " +
                       email + "\n";
            respuesta = respuesta + "-------Servicios------\n";
            foreach (string s in servicios)
            {
                respuesta = respuesta +s+ "\n";
            }
            return respuesta; 
        }
    }
}
